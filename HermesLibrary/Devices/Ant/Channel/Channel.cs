using HermesLibrary.Devices.Ant.Enum;
using HermesLibrary.Devices.Ant.Interfaces;
using HermesLibrary.Devices.Ant.Messages.Client;
using HermesLibrary.Devices.Ant.Messages.Device;

namespace HermesLibrary.Devices.Ant.Channel;

public class Channel
{
    private IAnt mDevice;

    public Channel(byte number, byte networkNumber, ChannelType type)
    {
        Number = number;
        NetworkNumber = networkNumber;
        Type = type;
    }

    public byte Number { get; init; }
    public byte NetworkNumber { get; init; }
    public ChannelType Type { get; init; }
    public ExtendedAssignmentType ExtendedAssignment { get; set; } = ExtendedAssignmentType.UNKNOWN;
    public ushort Period { get; set; }
    public byte Frequency { get; set; }

    public async Task Open(IAnt inter, IAntTransmitter device)
    {
        if (mDevice != null) await Close();

        mDevice = inter;
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