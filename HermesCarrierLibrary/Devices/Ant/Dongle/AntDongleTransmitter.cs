using HermesCarrierLibrary.Devices.Ant.Channel;
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

    public AntDongleTransmitter(ISerial device)
    {
        mDevice = device;

        //TODO This can not work since device maybe already opened
        device.Opened += OnOpen;
        device.Closed += OnClose;
    }

    /// <inheritdoc />
    public bool IsConnected => mDevice.IsConnected;

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
        Console.WriteLine($"Opening channel {channel.Number}");
        if (ActiveChannels.ContainsKey(channel.Number))
            throw new Exception("Channel already open");

        Console.WriteLine($"Opening channel {channel.Number}");
        await channel.Open(this);
        ActiveChannels.Add(channel.Number, channel);
    }

    /// <inheritdoc />
    public async Task CloseChannelAsync(IAntChannel channel)
    {
        if (!ActiveChannels.ContainsKey(channel.Number))
            throw new Exception("Channel not open");

        await channel.Close();
        ActiveChannels.Remove(channel.Number);
    }

    public void OnOpen(object? sender, System.EventArgs e)
    {
        StartReadThread();
    }

    public void OnClose(object? sender, System.EventArgs e)
    {
        // TODO: Implement
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

        Console.WriteLine($"Awaiting response for message type {message.MessageId:X2}");

        await SendMessageAsync(message);

        var response = await tcs.Task;

        Console.WriteLine($"Received response for message type {message.MessageId:X2}");
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

    private IAntMessage DecodeMessage(byte[] data)
    {
        var messageId = data[2];
        var message = AntService.DeviceBoundMessages.FirstOrDefault(x => x.MessageId == messageId);
        return message ?? new UnknownMessage(data);
    }

    private void StartReadThread()
    {
        var readThread = new Thread(async () => { await Start(); });
        readThread.Start();
    }

    private async Task Start()
    {
        while (mDevice.IsConnected)
        {
            var data = mDevice.Read();
            var message = await ReceiveMessageAsync(data);
            if (message is EventResponseMessage eventResponseMessage)
            {
                var source = mAwaitingMessages
                    .FirstOrDefault(x => x.Key.MessageId == eventResponseMessage.OriginalMessage).Value;
                if (source == null)
                    continue;

                source.SetResult(message);
            }
        }
    }
}