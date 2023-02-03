using HermesCarrierLibrary.Devices.Ant.Channel;
using HermesCarrierLibrary.Devices.Ant.Enum;
using HermesCarrierLibrary.Devices.Ant.Interfaces;
using HermesCarrierLibrary.Devices.Ant.Messages.Client;
using HermesCarrierLibrary.Devices.Ant.Messages.Device;
using HermesCarrierLibrary.Devices.Usb;
using HermesCarrierLibrary.Devices.Usb.Enum;
using Microsoft.Extensions.Logging;

namespace HermesCarrierLibrary.Devices.Ant.Dongle;

public class AntDongleTransmitter : IAntTransmitter
{
    private readonly IDictionary<IAntMessage, TaskCompletionSource<IAntMessage>>
        mAwaitingMessages = new Dictionary<IAntMessage, TaskCompletionSource<IAntMessage>>();

    private readonly ILogger<AntDongleTransmitter> mLogger = new LoggerFactory().CreateLogger<AntDongleTransmitter>();
    private readonly IUsbDevice mDevice;
    private readonly WeakEventManager mMessageReceivedEventManager = new();

    private IUsbInterface mUsbInterface;
    private IUsbEndpoint mReadEndpoint;
    private IUsbEndpoint mWriteEndpoint;
    
    private IUsbRequest mUsbRequestIn;

    private Thread mReadThread;

    public AntDongleTransmitter(IUsbDevice device)
    {
        mDevice = device;

        /*if (device.IsConnected)
            Start();
        else
            device.Opened += OnOpen;

        device.Closed += OnClose;*/
    }

    /// <inheritdoc />
    public bool IsConnected => throw new NotImplementedException();

    /// <inheritdoc />
    public string AntVersion { get; private set; }

    /// <inheritdoc />
    public string SerialNumber { get; private set; }

    /// <inheritdoc />
    public IEnumerable<Capabilities> Capabilities { get; private set; }

    /// <inheritdoc />
    public IDictionary<byte, IAntChannel> ActiveChannels { get; } = new Dictionary<byte, IAntChannel>();

    public async Task Open()
    {
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

        mLogger.LogInformation("ANT+ Dongle connected successfully ({0})", mDevice.DeviceName);
        mOpenEventManager.HandleEvent(this, EventArgs.Empty, nameof(Opened));
    }

    /// <inheritdoc />
    public async Task SetNetworkKeyAsync(byte networkNumber, byte[] key)
    {
        await SendMessageAsync(new SetNetworkKeyMessage(networkNumber, key));
    }

    /// <inheritdoc />
    public async Task OpenChannelAsync(IAntChannel channel)
    {
        if (ActiveChannels.ContainsKey(channel.Number))
            throw new Exception("Channel is already bound to this transmitter");

        await channel.Open();

        MessageReceived += channel.OnMessageReceived;
        ActiveChannels.Add(channel.Number, channel);
    }

    /// <inheritdoc />
    public async Task CloseChannelAsync(IAntChannel channel)
    {
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
        await Task.Run(() => { mDevice.Write(message.Encode()); });
    }

    /// <inheritdoc />
    public async Task<IAntMessage> AwaitMessageAsync(IAntMessage message)
    {
        var tcs = new TaskCompletionSource<IAntMessage>();
        mAwaitingMessages.Add(message, tcs);

        await SendMessageAsync(message);

        var response = await tcs.Task;
        return response;
    }

    /// <inheritdoc />
    public Task<T> AwaitMessageOfTypeAsync<T>(IAntMessage message) where T : IAntMessage
    {
        return Task.Run(() =>
        {
            var response = AwaitMessageAsync(message);
            if (response.Result is T t) return t;

            throw new Exception("Message type mismatch");
        });
    }

    /// <inheritdoc />
    public Task<IAntMessage> ReceiveMessageAsync(byte[] data)
    {
        var message = DecodeMessage(data);
        if (message is not UnknownMessage) message.Decode(data);

        return Task.FromResult(message);
    }

    public event EventHandler<AntMessageReceivedEventArgs> MessageReceived
    {
        add => mMessageReceivedEventManager.AddEventHandler(value);
        remove => mMessageReceivedEventManager.RemoveEventHandler(value);
    }

    public void OnOpen(object? sender, System.EventArgs e)
    {
        Start();
    }

    public void OnClose(object? sender, System.EventArgs e)
    {
    }

    private IAntMessage DecodeMessage(byte[] data)
    {
        Console.WriteLine("Received: " + BitConverter.ToString(data).Replace("-", " "));
        var messageId = data[2];
        var message = AntService.DeviceBoundMessages.FirstOrDefault(x => x.MessageId == messageId);
        return message ?? new UnknownMessage(data);
    }

    private void Start()
    {
        mReadThread = new Thread(async () => { await StartReadThread(); });
        mReadThread.Start();

        Task.Run(async () =>
        {
            Thread.Sleep(100);

            await SendMessageAsync(new RequestMessage(RequestMessageType.ANT_VERSION));
            await SendMessageAsync(new RequestMessage(RequestMessageType.SERIAL_NUMBER));
            await SendMessageAsync(new RequestMessage(RequestMessageType.CAPABILITIES));
        });
    }

    private async Task StartReadThread()
    {
        while (mDevice.IsConnected)
        {
            var data = mDevice.Read();
            if (data == null || data.Length == 0) break;

            var message = await ReceiveMessageAsync(data);
            switch (message)
            {
                case EventResponseMessage eventResponseMessage:
                {
                    var (key, value) = mAwaitingMessages
                        .FirstOrDefault(x => x.Key.MessageId == eventResponseMessage.OriginalMessage);

                    if (value != null)
                    {
                        mAwaitingMessages.Remove(key);
                        if (!value.TrySetResult(message))
                        {
                            Console.WriteLine("Failed to set result");
                        }
                    }

                    break;
                }
                case AntVersionMessage versionMessage:
                    AntVersion = versionMessage.Version;
                    break;
                case SerialNumberMessage serialNumberMessage:
                    SerialNumber = BitConverter.ToString(serialNumberMessage.SerialNumber).Replace("-", "");
                    break;
                case CapabilitiesMessage capabilitiesMessage:
                    Capabilities = capabilitiesMessage.Capabilities;
                    break;
            }

            mMessageReceivedEventManager.HandleEvent(this, new AntMessageReceivedEventArgs(message),
                nameof(MessageReceived));
        }

        foreach (var channel in ActiveChannels.Values)
        {
            await CloseChannelAsync(channel);
        }
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return mDevice.VendorId ^ mDevice.ProductId;
    }
}