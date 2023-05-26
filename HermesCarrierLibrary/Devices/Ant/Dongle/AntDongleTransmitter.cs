using HermesCarrierLibrary.Devices.Ant.Channel;
using HermesCarrierLibrary.Devices.Ant.Enum;
using HermesCarrierLibrary.Devices.Ant.EventArgs;
using HermesCarrierLibrary.Devices.Ant.Interfaces;
using HermesCarrierLibrary.Devices.Ant.Messages.Client;
using HermesCarrierLibrary.Devices.Ant.Messages.Device;
using HermesCarrierLibrary.Devices.Usb;
using HermesCarrierLibrary.Devices.Usb.Enum;
using Microsoft.Extensions.Logging;

namespace HermesCarrierLibrary.Devices.Ant.Dongle;

public class AntDongleTransmitter : IAntTransmitter
{
    private readonly IDictionary<IAntMessage, TaskCompletionSource<IAntMessage>> mAwaitingMessages = new Dictionary<IAntMessage, TaskCompletionSource<IAntMessage>>();

    private readonly IUsbDevice mDevice;

    private readonly ILogger<AntDongleTransmitter> mLogger = new LoggerFactory().CreateLogger<AntDongleTransmitter>();

    private readonly WeakEventManager mMessageReceivedEventManager = new();
    private readonly WeakEventManager mTransmitterStatusChangedEventManager = new();
    private IUsbEndpoint mReadEndpoint;

    private Thread mReadThread;

    private IUsbInterface mUsbInterface;

    private IUsbRequest mUsbRequestIn;
    private IUsbEndpoint mWriteEndpoint;

    public AntDongleTransmitter(IUsbDevice device)
    {
        mDevice = device;
    }

    public event EventHandler<AntMessageReceivedEventArgs> MessageReceived
    {
        add => mMessageReceivedEventManager.AddEventHandler(value);
        remove => mMessageReceivedEventManager.RemoveEventHandler(value);
    }

    public event EventHandler<AntTransmitterStatusChangedEventArgs> TransmitterStatusChanged
    {
        add => mTransmitterStatusChangedEventManager.AddEventHandler(value);
        remove => mTransmitterStatusChangedEventManager.RemoveEventHandler(value);
    }

    /// <inheritdoc />
    public bool IsConnected { get; private set; }

    /// <inheritdoc />
    public string AntVersion { get; private set; }

    /// <inheritdoc />
    public string SerialNumber { get; private set; }

    /// <inheritdoc />
    public IEnumerable<Capabilities> Capabilities { get; private set; }

    /// <inheritdoc />
    public IDictionary<byte, IAntChannel> ActiveChannels { get; } = new Dictionary<byte, IAntChannel>();

    /// <inheritdoc />
    public async Task OpenAsync()
    {
        if (!mDevice.HasPermission) await mDevice.RequestPermissionAsync();

        mUsbInterface = mDevice.Interfaces.First();
        for (var i = 0; i < mUsbInterface.Endpoints.Count(); i++)
        {
            var endpoint = mUsbInterface.Endpoints.ElementAt(i);
            if (endpoint is not { Type: UsbType.XFerBulk }) continue;

            if (endpoint.Direction == UsbDirection.In)
                mReadEndpoint = endpoint;
            else
                mWriteEndpoint = endpoint;
        }

        await mDevice.OpenAsync();
        await mDevice.ClaimInterfaceAsync(mUsbInterface);

        mUsbRequestIn = await mDevice.CreateRequestAsync();
        mUsbRequestIn.Initialize(mDevice, mReadEndpoint);
        IsConnected = true;

        mTransmitterStatusChangedEventManager.HandleEvent(this, new AntTransmitterStatusChangedEventArgs(this, Status.Connected), nameof(OpenAsync));

        Start();

        mLogger.LogInformation("ANT+ Dongle connected successfully ({0})", mDevice.DeviceName);
    }

    /// <inheritdoc />
    public async Task CloseAsync()
    {
        foreach (var channel in ActiveChannels.Values) await CloseChannelAsync(channel);

        IsConnected = false;

        try
        {
            mReadThread.Interrupt();
        }
        catch (Exception e)
        {
            mLogger.LogError(e, "Failed to interrupt read thread");
        }

        // Wait for the read thread to finish
        Thread.Sleep(1000);

        mUsbRequestIn?.Close();
        mUsbRequestIn = null;

        mDevice?.Close();
        IsConnected = false;

        mTransmitterStatusChangedEventManager.HandleEvent(this, new AntTransmitterStatusChangedEventArgs(this, Status.Disconnected), nameof(CloseAsync));

        mLogger.LogInformation("ANT+ Dongle disconnected successfully ({0})", mDevice.DeviceName);
    }

    /// <inheritdoc />
    public async Task SetNetworkKeyAsync(byte networkNumber, byte[] key)
    {
        if (!IsConnected)
            throw new Exception("Cannot set network key because the transmitter is not connected");

        await SendMessageAsync(new SetNetworkKeyMessage(networkNumber, key));
    }

    /// <inheritdoc />
    public async Task OpenChannelAsync(IAntChannel channel)
    {
        if (!IsConnected)
        {
            mLogger.LogWarning("Cannot open channel {0} because the transmitter is not connected", channel.Number);
            return;
        }

        if (ActiveChannels.ContainsKey(channel.Number))
            throw new Exception("Channel is already bound to this transmitter");

        await channel.Open();

        MessageReceived += channel.OnMessageReceived;
        ActiveChannels.Add(channel.Number, channel);
    }

    /// <inheritdoc />
    public async Task CloseChannelAsync(IAntChannel channel)
    {
        if (!IsConnected)
        {
            mLogger.LogWarning("Cannot close channel {0} because the transmitter is not connected", channel.Number);
            return;
        }

        if (!IsChannelOpen(channel.Number))
            throw new Exception("Channel is not bound to this transmitter");

        MessageReceived -= channel.OnMessageReceived;
        await channel.Close();
        ActiveChannels.Remove(channel.Number);
    }

    /// <inheritdoc />
    public bool IsChannelOpen(byte channelNumber)
    {
        return ActiveChannels.ContainsKey(channelNumber);
    }

    /// <inheritdoc />
    public async Task SendMessageAsync(IAntMessage message)
    {
        if (!IsConnected)
            throw new Exception("Cannot send message because the transmitter is not connected");

        var data = message.Encode();
        var transfer = new UsbBulkTransfer(mWriteEndpoint, data, data.Length, 1000);
        await mDevice.BulkTransferAsync(transfer);
    }

    /// <inheritdoc />
    public async Task<IAntMessage> AwaitMessageAsync(IAntMessage message)
    {
        if (!IsConnected)
            throw new Exception("Cannot await message because the transmitter is not connected");

        var tcs = new TaskCompletionSource<IAntMessage>();
        mAwaitingMessages.Add(message, tcs);

        await SendMessageAsync(message);

        var response = await tcs.Task;
        return response;
    }

    /// <inheritdoc />
    public Task<T> AwaitMessageOfTypeAsync<T>(IAntMessage message) where T : IAntMessage
    {
        if (!IsConnected)
            throw new Exception("Cannot await message because the transmitter is not connected");

        return Task.Run(
            () =>
            {
                var response = AwaitMessageAsync(message);
                if (response.Result is T t) return t;

                throw new Exception("Message type mismatch");
            });
    }

    /// <inheritdoc />
    public Task<IAntMessage> ReceiveMessageAsync(byte[] data)
    {
        if (!IsConnected)
            throw new Exception("Cannot receive message because the transmitter is not connected");

        var message = DecodeMessage(data);
        if (message is not UnknownMessage) message.Decode(data);

        return Task.FromResult(message);
    }

    private IAntMessage DecodeMessage(byte[] data)
    {
        var messageId = data[2];
        var message = AntService.DeviceBoundMessages.FirstOrDefault(x => x.MessageId == messageId);
        return message ?? new UnknownMessage(data);
    }

    private void Start()
    {
        mReadThread = new Thread(async () => { await StartReadThread(); });
        mReadThread.Start();

        Task.Run(
            async () =>
            {
                Thread.Sleep(100);

                await SendMessageAsync(new RequestMessage(RequestMessageType.ANT_VERSION));
                await SendMessageAsync(new RequestMessage(RequestMessageType.SERIAL_NUMBER));
                await SendMessageAsync(new RequestMessage(RequestMessageType.CAPABILITIES));
            });
    }

    private async Task StartReadThread()
    {
        while (IsConnected)
        {
            try
            {
                var data = await Read();
                if (data == null || data.Length == 0) break;

                var message = await ReceiveMessageAsync(data);
                switch (message)
                {
                    case EventResponseMessage eventResponseMessage:
                    {
                        var (key, value) = mAwaitingMessages.FirstOrDefault(x => x.Key.MessageId == eventResponseMessage.OriginalMessage);

                        if (value != null)
                        {
                            mAwaitingMessages.Remove(key);
                            if (!value.TrySetResult(message))
                                mLogger.LogWarning("Failed to set result for awaiting message");
                        }

                        break;
                    }
                    case AntVersionMessage versionMessage:
                        AntVersion = versionMessage.Version;
                        break;
                    case SerialNumberMessage serialNumberMessage:
                        SerialNumber = BitConverter.ToString(serialNumberMessage.SerialNumber)
                            .Replace("-", "");
                        break;
                    case CapabilitiesMessage capabilitiesMessage:
                        Capabilities = capabilitiesMessage.Capabilities;
                        break;
                }

                mMessageReceivedEventManager.HandleEvent(this, new AntMessageReceivedEventArgs(message), nameof(MessageReceived));
            }
            catch (Exception e)
            {
                mLogger.LogError(e, "Failed to read from ANT+ dongle");
            }
        }
    }

    private async Task<byte[]> Read()
    {
        if (!IsConnected) throw new Exception("Device not connected");

        var buffer = new byte[mReadEndpoint.MaxPacketSize];
        if (mUsbRequestIn.Queue(buffer, buffer.Length)) return await mUsbRequestIn.RequestWaitAsync(mDevice);

        mLogger.LogError("Failed to queue USB request");
        await CloseAsync();
        return null;
    }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
        return obj is AntDongleTransmitter transmitter && transmitter.mDevice.Equals(mDevice);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return mDevice.GetHashCode() ^ 31;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{{\"{nameof(SerialNumber)}\":\"{SerialNumber}\"," + $"\"{nameof(AntVersion)}\":\"{AntVersion}\"," + $"\"{nameof(Capabilities)}\":\"{Capabilities}\"}}";
    }
}