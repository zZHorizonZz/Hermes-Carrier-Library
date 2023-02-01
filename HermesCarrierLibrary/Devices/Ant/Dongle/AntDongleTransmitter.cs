using HermesCarrierLibrary.Devices.Ant.Channel;
using HermesCarrierLibrary.Devices.Ant.Enum;
using HermesCarrierLibrary.Devices.Ant.Interfaces;
using HermesCarrierLibrary.Devices.Ant.Messages.Client;
using HermesCarrierLibrary.Devices.Ant.Messages.Device;
using HermesCarrierLibrary.Devices.Shared;

namespace HermesCarrierLibrary.Devices.Ant.Dongle;

public class AntDongleTransmitter : IAntTransmitter
{
    private readonly IDictionary<IAntMessage, TaskCompletionSource<IAntMessage>>
        mAwaitingMessages = new Dictionary<IAntMessage, TaskCompletionSource<IAntMessage>>();

    private readonly ISerial mDevice;
    private readonly WeakEventManager mMessageReceivedEventManager = new();

    private Thread mReadThread;

    public AntDongleTransmitter(ISerial device)
    {
        mDevice = device;

        if (device.IsConnected)
            Start();
        else
            device.Opened += OnOpen;

        device.Closed += OnClose;
    }

    /// <inheritdoc />
    public bool IsConnected => mDevice.IsConnected;

    /// <inheritdoc />
    public string AntVersion { get; private set; }

    /// <inheritdoc />
    public string SerialNumber { get; private set; }

    /// <inheritdoc />
    public IEnumerable<Capabilities> Capabilities { get; private set; }

    /// <inheritdoc />
    public IDictionary<byte, IAntChannel> ActiveChannels { get; } = new Dictionary<byte, IAntChannel>();


    /// <inheritdoc />
    public async Task SetNetworkKeyAsync(byte networkNumber, byte[] key)
    {
        await SendMessageAsync(new SetNetworkKeyMessage(networkNumber, key));
    }

    /// <inheritdoc />
    public async Task OpenChannelAsync(IAntChannel channel)
    {
        if (ActiveChannels.ContainsKey(channel.Number))
            throw new Exception("Channel already open");

        await channel.Open();

        MessageReceived += channel.OnMessageReceived;
        ActiveChannels.Add(channel.Number, channel);
    }

    /// <inheritdoc />
    public async Task CloseChannelAsync(IAntChannel channel)
    {
        if (!ActiveChannels.ContainsKey(channel.Number))
            throw new Exception("Channel not open");

        MessageReceived -= channel.OnMessageReceived;
        await channel.Close();
        ActiveChannels.Remove(channel.Number);
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
            Console.WriteLine("Starting");
            Thread.Sleep(250);
            Console.WriteLine("Sending reset");

            await SendMessageAsync(new RequestMessage(RequestMessageType.ANT_VERSION));
            await SendMessageAsync(new RequestMessage(RequestMessageType.SERIAL_NUMBER));
            await SendMessageAsync(new RequestMessage(RequestMessageType.CAPABILITIES));

            Console.WriteLine("Sent reset");
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
                    var source = mAwaitingMessages
                        .FirstOrDefault(x => x.Key.MessageId == eventResponseMessage.OriginalMessage).Value;
                    if (source == null)
                        continue;

                    source.SetResult(message);
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
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return mDevice.VendorId ^ mDevice.ProductId;
    }
}