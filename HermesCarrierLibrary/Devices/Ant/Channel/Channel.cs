using HermesCarrierLibrary.Devices.Ant.Enum;
using HermesCarrierLibrary.Devices.Ant.Interfaces;
using HermesCarrierLibrary.Devices.Ant.Messages.Client;
using HermesCarrierLibrary.Devices.Ant.Messages.Device;
using Microsoft.Extensions.Logging;

namespace HermesCarrierLibrary.Devices.Ant.Channel;

public class Channel : IAntChannel
{
    private readonly ILogger mLogger = new LoggerFactory().CreateLogger<Channel>();
    private readonly WeakEventManager mMessageReceivedEventManager = new();

    private IAntTransmitter? mTransmitter;

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

    public event EventHandler<AntMessageReceivedEventArgs> MessageReceived
    {
        add => mMessageReceivedEventManager.AddEventHandler(value);
        remove => mMessageReceivedEventManager.RemoveEventHandler(value);
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
            mLogger.LogError("Failed to assign ANT channel {result}", result);
            return;
        }

        mLogger.LogInformation("ANT Channel assigned successfully");
    }

    public async Task Open()
    {
        var result = (await AwaitMessageOfTypeAsync<EventResponseMessage>(new OpenChannelMessage())).Type;
        if (result != EventResponseType.RESPONSE_NO_ERROR)
        {
            mLogger.LogError("Failed to open ANT channel {result}", result);
            return;
        }

        mLogger.LogInformation("ANT Channel opened successfully");
    }

    public async Task Close()
    {
        if (mTransmitter == null)
            throw new InvalidOperationException("Channel is not open");

        await AwaitMessageOfTypeAsync<EventResponseMessage>(new UnAssignChannelMessage());
        await AwaitMessageOfTypeAsync<EventResponseMessage>(new CloseChannelMessage());
        mLogger.LogInformation("ANT Channel closed successfully");
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
        if (e.Message.ChannelNumber == Number)
        {
            mMessageReceivedEventManager.HandleEvent(this, e, nameof(MessageReceived));
        }
    }
}