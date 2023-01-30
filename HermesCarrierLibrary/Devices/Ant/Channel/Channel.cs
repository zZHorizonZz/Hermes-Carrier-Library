using HermesCarrierLibrary.Devices.Ant.Enum;
using HermesCarrierLibrary.Devices.Ant.Interfaces;
using HermesCarrierLibrary.Devices.Ant.Messages.Client;
using HermesCarrierLibrary.Devices.Ant.Messages.Device;

namespace HermesCarrierLibrary.Devices.Ant.Channel;

public class Channel : IAntChannel
{
    private readonly WeakEventManager mMessageReceivedEventManager = new();

    private IAntTransmitter? mTransmitter;

    public event EventHandler<AntMessageReceivedEventArgs> MessageReceived
    {
        add => mMessageReceivedEventManager.AddEventHandler(value);
        remove => mMessageReceivedEventManager.RemoveEventHandler(value);
    }

    public Channel()
    {
    }

    public Channel(byte number, byte networkNumber, ChannelType type, ExtendedAssignmentType extendedAssignment,
        ushort period, byte frequency)
    {
        Number = number;
        NetworkNumber = networkNumber;
        Type = type;
        ExtendedAssignment = extendedAssignment;
        Period = period;
        Frequency = frequency;
    }

    /// <inheritdoc />
    public byte Number { get; set; }

    /// <inheritdoc />
    public byte NetworkNumber { get; set; }

    /// <inheritdoc />
    public ChannelType Type { get; set; } = ChannelType.ReceiveChannel;

    /// <inheritdoc />
    public ExtendedAssignmentType ExtendedAssignment { get; set; } = ExtendedAssignmentType.UNKNOWN;

    /// <inheritdoc />
    public ushort Period { get; set; } = 0x2000;

    /// <inheritdoc />
    public byte Frequency { get; set; } = 0x03;

    /// <inheritdoc />
    public async Task AssignChannel(IAntTransmitter transmitter)
    {
        mTransmitter = transmitter;

        var result =
            (await AwaitMessageOfTypeAsync<EventResponseMessage>(new AssignChannelMessage(
                Type,
                NetworkNumber,
                ExtendedAssignment))).Type;

        if (result != EventResponseType.RESPONSE_NO_ERROR)
        {
            Console.WriteLine($"Failed to assign channel {result}");
            return;
        }

        Console.WriteLine("Channel assigned");
    }

    public async Task Open()
    {
        var result = (await AwaitMessageOfTypeAsync<EventResponseMessage>(new OpenChannelMessage())).Type;
        Console.WriteLine($"Result: {result}");
        if (result != EventResponseType.RESPONSE_NO_ERROR)
        {
            Console.WriteLine($"Failed to open channel {result}");
            return;
        }

        Console.WriteLine("Channel opened");
    }

    public async Task Close()
    {
        if (mTransmitter == null)
            throw new InvalidOperationException("Channel is not open");

        await AwaitMessageOfTypeAsync<EventResponseMessage>(new UnAssignChannelMessage());
        await AwaitMessageOfTypeAsync<EventResponseMessage>(new CloseChannelMessage());
    }

    /// <inheritdoc />
    public async Task SendMessageAsync(IAntMessage message)
    {
        if (mTransmitter == null)
            throw new InvalidOperationException("Channel is not open");

        message.ChannelNumber = Number;
        await mTransmitter.SendMessageAsync(message);
    }

    /// <inheritdoc />
    public async Task<IAntMessage> AwaitMessageAsync(IAntMessage message)
    {
        Console.WriteLine("Awaiting message");
        if (mTransmitter == null)
            throw new InvalidOperationException("Channel is not open");

        message.ChannelNumber = Number;
        return await mTransmitter.AwaitMessageAsync(message);
    }

    /// <inheritdoc />
    public async Task<T> AwaitMessageOfTypeAsync<T>(IAntMessage message) where T : IAntMessage
    {
        if (mTransmitter == null)
            throw new InvalidOperationException("Channel is not open");

        message.ChannelNumber = Number;
        return await mTransmitter.AwaitMessageOfTypeAsync<T>(message);
    }

    /// <inheritdoc />
    public async Task<IAntMessage> ReceiveMessageAsync(byte[] data)
    {
        if (mTransmitter == null)
            throw new InvalidOperationException("Channel is not open");

        return await mTransmitter.ReceiveMessageAsync(data);
    }

    /// <inheritdoc />
    public void OnMessageReceived(object sender, AntMessageReceivedEventArgs e)
    {
        mMessageReceivedEventManager.HandleEvent(this, e, nameof(MessageReceived));
    }
}