using HermesLibrary.Devices.Ant.Interfaces;
using HermesLibrary.Devices.Ant.Messages.Device;
using HermesLibrary.Devices.Shared;

namespace HermesLibrary.Devices.Ant.Dongle;

public class AntDongleTransmitter : IAnt, IAntTransmitter
{
    private readonly IDictionary<IAntMessage, TaskCompletionSource<IAntMessage>>
        mAwaitingMessages = new Dictionary<IAntMessage, TaskCompletionSource<IAntMessage>>();

    private readonly ISerial mDevice;

    public AntDongleTransmitter(ISerial device)
    {
        mDevice = device;

        device.Opened += OnOpen;
        device.Closed += OnClose;
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
    public IAntMessage ReceiveMessageAsync(byte[] data)
    {
        var message = DecodeMessage(data);
        if (message is not UnknownMessage) message.Decode(data);

        return message;
    }

    /// <inheritdoc />
    public ushort DeviceNumber { get; }

    /// <inheritdoc />
    public bool IsSlave => false;

    /// <inheritdoc />
    public byte DeviceType { get; }

    /// <inheritdoc />
    public byte TransmissionType { get; }

    public void OnOpen(object? sender, System.EventArgs e)
    {
        StartReadThread();
    }

    public void OnClose(object? sender, System.EventArgs e)
    {
        // TODO: Implement
    }

    private IAntMessage DecodeMessage(byte[] data)
    {
        var messageId = data[2];
        var message = AntService.DeviceBoundMessages.FirstOrDefault(x => x.MessageId == messageId);
        return message ?? new UnknownMessage(data);
    }

    private void StartReadThread()
    {
        var readThread = new Thread(Start);
        readThread.Start();
    }

    private void Start()
    {
        while (mDevice.IsConnected)
        {
            var data = mDevice.Read();
            var message = ReceiveMessageAsync(data);
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