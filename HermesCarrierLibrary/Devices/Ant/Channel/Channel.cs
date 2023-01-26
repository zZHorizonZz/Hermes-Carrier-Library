using HermesCarrierLibrary.Devices.Ant.Enum;
using HermesCarrierLibrary.Devices.Ant.Interfaces;
using HermesCarrierLibrary.Devices.Ant.Messages.Client;
using HermesCarrierLibrary.Devices.Ant.Messages.Device;

namespace HermesCarrierLibrary.Devices.Ant.Channel;

public class Channel : IAntChannel
{
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

    /// <inheritdoc />
    public byte Number { get; set; } = 0;

    /// <inheritdoc />
    public byte NetworkNumber { get; set; } = 0;

    /// <inheritdoc />
    public ChannelType Type { get; set; } = ChannelType.ReceiveChannel;

    /// <inheritdoc />
    public ExtendedAssignmentType ExtendedAssignment { get; set; } = ExtendedAssignmentType.UNKNOWN;

    /// <inheritdoc />
    public ushort Period { get; set; } = 0x2000;

    /// <inheritdoc />
    public byte Frequency { get; set; } = 0x03;

    /// <inheritdoc />
    public ushort DeviceNumber { get; set; }

    /// <inheritdoc />
    public bool IsSlave { get; set; }

    /// <inheritdoc />
    public byte DeviceType { get; set; }

    /// <inheritdoc />
    public byte TransmissionType { get; set; }

    public async Task Open(IAntTransmitter transmitter)
    {
        Console.WriteLine("Opening channel");
        if (transmitter == null)
            throw new ArgumentNullException(nameof(transmitter));

        Console.WriteLine("Assigning channel");
        if (mTransmitter != null) await Close();

        Console.WriteLine("Assigning channel");
        mTransmitter = transmitter;
        var result =
            (await AwaitMessageOfTypeAsync<EventResponseMessage>(new AssignChannelMessage(Type, NetworkNumber,
                ExtendedAssignment))).Type;

        Console.WriteLine("Assigning channel");
        if (result != EventResponseType.RESPONSE_NO_ERROR)
        {
            Console.WriteLine($"Failed to assign channel {result}");
            return;
        }

        result = (await AwaitMessageOfTypeAsync<EventResponseMessage>(new SetChannelIdMessage(DeviceNumber,
            IsSlave,
            DeviceType,
            TransmissionType))).Type;

        if (result != EventResponseType.RESPONSE_NO_ERROR)
        {
            Console.WriteLine($"Failed to set channel id {result}");
            return;
        }

        result = (await AwaitMessageOfTypeAsync<EventResponseMessage>(new ChannelMessagingPeriodMessage(Period)))
            .Type;

        if (result != EventResponseType.RESPONSE_NO_ERROR)
        {
            Console.WriteLine($"Failed to set channel period {result}");
            return;
        }

        result = (await AwaitMessageOfTypeAsync<EventResponseMessage>(new ChannelRFFrequencyMessage(Frequency)))
            .Type;

        if (result != EventResponseType.RESPONSE_NO_ERROR)
        {
            Console.WriteLine($"Failed to set channel frequency {result}");
            return;
        }

        result = (await AwaitMessageOfTypeAsync<EventResponseMessage>(new OpenChannelMessage())).Type;
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
}