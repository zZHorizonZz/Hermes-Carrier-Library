using HermesCarrierLibrary.Devices.Ant.Channel;
using HermesCarrierLibrary.Devices.Ant.Enum;
using HermesCarrierLibrary.Devices.Ant.Interfaces;
using HermesCarrierLibrary.Devices.Ant.Messages.Client;
using HermesCarrierLibrary.Devices.Ant.Messages.Device;

namespace HermesCarrierDemo;

public class TestDevice
{
    /// <summary>
    ///     Gets the unique device number assigned to the ANT transmitter.
    /// </summary>
    public ushort DeviceNumber { get; init; } = 0x7d0;

    /// <summary>
    ///     Gets a value indicating whether the ANT transmitter is configured as a slave or a master device.
    /// </summary>
    public bool IsSlave { get; init; }

    /// <summary>
    ///     Gets the type of device the ANT transmitter is communicating with.
    /// </summary>
    public byte DeviceType { get; init; } = 0x05;

    /// <summary>
    ///     Gets the type of transmission being used by the ANT transmitter, such as asynchronous or synchronous.
    /// </summary>
    public byte TransmissionType { get; init; } = 0x01;

    public TestDevice()
    {
    }

    public TestDevice(ushort deviceNumber, bool isSlave, byte deviceType, byte transmissionType)
    {
        DeviceNumber = deviceNumber;
        IsSlave = isSlave;
        DeviceType = deviceType;
        TransmissionType = transmissionType;
    }

    public async Task Open(IAntTransmitter transmitter, IAntChannel channel)
    {
        await transmitter.SendMessageAsync(new ResetSystemMessage());
        Thread.Sleep(500);
        await transmitter.SendMessageAsync(new SetNetworkKeyMessage(1,
            new byte[] { 0xF9, 0xED, 0x22, 0xB8, 0xFD, 0x56, 0x67, 0xCD }));

        await channel.AssignChannel(transmitter);

        var result = (await channel.AwaitMessageOfTypeAsync<EventResponseMessage>(new SetChannelIdMessage(DeviceNumber,
            !IsSlave,
            DeviceType,
            TransmissionType))).Type;

        if (result != EventResponseType.RESPONSE_NO_ERROR)
        {
            Console.WriteLine($"Failed to set channel id {result}");
            return;
        }

        result = (await channel.AwaitMessageOfTypeAsync<EventResponseMessage>(
            new ChannelMessagingPeriodMessage(channel.Period))).Type;

        if (result != EventResponseType.RESPONSE_NO_ERROR)
        {
            Console.WriteLine($"Failed to set channel period {result}");
            return;
        }

        result = (await channel.AwaitMessageOfTypeAsync<EventResponseMessage>(
            new ChannelRFFrequencyMessage(channel.Frequency))).Type;

        if (result != EventResponseType.RESPONSE_NO_ERROR)
        {
            Console.WriteLine($"Failed to set channel frequency {result}");
            return;
        }

        await transmitter.OpenChannelAsync(channel);
    }
}