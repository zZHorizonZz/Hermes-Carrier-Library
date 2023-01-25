using HermesLibrary.Devices.Ant.Enum;
using HermesLibrary.Devices.Ant.Interfaces;
using HermesLibrary.Devices.Ant.Messages.Client;
using HermesLibrary.Devices.Ant.Messages.Device;

namespace HermesLibrary.Devices.Ant.Channel;

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
    public byte Number { get; } = 0;

    /// <inheritdoc />
    public byte NetworkNumber { get; } = 0;

    /// <inheritdoc />
    public ChannelType Type { get; } = ChannelType.ReceiveChannel;

    /// <inheritdoc />
    public ExtendedAssignmentType ExtendedAssignment { get; } = ExtendedAssignmentType.UNKNOWN;

    /// <inheritdoc />
    public ushort Period { get; } = 0x2000;

    /// <inheritdoc />
    public byte Frequency { get; } = 0x03;

    public async Task Open(IAntTransmitter transmitter)
    {
        if (transmitter == null)
            throw new ArgumentNullException(nameof(transmitter));

        if (mTransmitter != null) await Close();

        mTransmitter = transmitter;
        var result = (await mDevice.AwaitMessageOfTypeAsync<EventResponseMessage>(
            new AssignChannelMessage(Number, Type, NetworkNumber, ExtendedAssignment))).Type;

        if (result != EventResponseType.RESPONSE_NO_ERROR)
        {
            Console.WriteLine($"Failed to assign channel {result}");
            return;
        }

        result = (await mDevice.AwaitMessageOfTypeAsync<EventResponseMessage>(new SetChannelIdMessage(Number,
            device.DeviceNumber, device.IsSlave, device.DeviceType,
            device.TransmissionType))).Type;

        if (result != EventResponseType.RESPONSE_NO_ERROR)
        {
            Console.WriteLine($"Failed to set channel id {result}");
            return;
        }

        result = (await mDevice.AwaitMessageOfTypeAsync<EventResponseMessage>(
            new ChannelMessagingPeriodMessage(Number, Period))).Type;

        if (result != EventResponseType.RESPONSE_NO_ERROR)
        {
            Console.WriteLine($"Failed to set channel period {result}");
            return;
        }

        result = (await mDevice.AwaitMessageOfTypeAsync<EventResponseMessage>(
            new ChannelRFFrequencyMessage(Number, Frequency))).Type;

        if (result != EventResponseType.RESPONSE_NO_ERROR)
        {
            Console.WriteLine($"Failed to set channel frequency {result}");
            return;
        }

        result = (await mDevice.AwaitMessageOfTypeAsync<EventResponseMessage>(new OpenChannelMessage(Number))).Type;
        if (result != EventResponseType.RESPONSE_NO_ERROR)
        {
            Console.WriteLine($"Failed to open channel {result}");
            return;
        }

        Console.WriteLine("Channel opened");
    }

    public async Task Close()
    {
        await mDevice.AwaitMessageOfTypeAsync<EventResponseMessage>(new UnAssignChannelMessage(Number));
        await mDevice.AwaitMessageOfTypeAsync<EventResponseMessage>(new CloseChannelMessage(Number));
    }

    public async Task SendMessage(IAntMessage message)
    {
        if (mDevice == null)
            throw new InvalidOperationException("Channel is not open");

        await mDevice.SendMessageAsync(message);
    }
}