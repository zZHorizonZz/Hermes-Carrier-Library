using HermesCarrierLibrary.Devices.Ant;
using HermesCarrierLibrary.Devices.Ant.Channel;
using HermesCarrierLibrary.Devices.Ant.Enum;
using HermesCarrierLibrary.Devices.Ant.Interfaces;
using HermesCarrierLibrary.Devices.Ant.Messages.Client;
using HermesCarrierLibrary.Devices.Ant.Messages.Device;
using ChannelIdMessage = HermesCarrierLibrary.Devices.Ant.Messages.Client.ChannelIdMessage;

namespace HermesCarrierDemo;

public class TestDevice
{
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

    public event EventHandler<ValueReceivedEventArgs> ValueReceived;

    public async Task Open(IAntTransmitter transmitter, IAntChannel channel)
    {
        await transmitter.SendMessageAsync(new ResetSystemMessage());
        Thread.Sleep(500);
        await transmitter.SendMessageAsync(new SetNetworkKeyMessage(1,
            new byte[] { 0xF9, 0xED, 0x22, 0xB8, 0xFD, 0x56, 0x67, 0xCD }));

        await channel.AssignChannel(transmitter);

        var result = (await channel.AwaitMessageOfTypeAsync<EventResponseMessage>(new ChannelIdMessage(DeviceNumber,
            !IsSlave,
            DeviceType,
            TransmissionType))).Type;

        if (result != EventResponseType.RESPONSE_NO_ERROR)
        {
            Console.WriteLine($"Failed to set channel id {result}");
            return;
        }

        result = (await channel.AwaitMessageOfTypeAsync<EventResponseMessage>(
            new ChannelPeriodMessage(channel.Period))).Type;

        if (result != EventResponseType.RESPONSE_NO_ERROR)
        {
            Console.WriteLine($"Failed to set channel period {result}");
            return;
        }

        result = (await channel.AwaitMessageOfTypeAsync<EventResponseMessage>(
            new ChannelRfFrequencyMessage(channel.Frequency))).Type;

        if (result != EventResponseType.RESPONSE_NO_ERROR)
        {
            Console.WriteLine($"Failed to set channel frequency {result}");
            return;
        }

        channel.MessageReceived += OnMessageReceived;
        await transmitter.OpenChannelAsync(channel);
    }

    private void OnMessageReceived(object sender, AntMessageReceivedEventArgs e)
    {
        Console.WriteLine($"Received message: {e.Message}");
        if (e.Message is AcknowledgedDataMessage message)
            DecodeAndPrintData(message.Data);
    }

    private void DecodeAndPrintData(byte[] data)
    {
        Console.WriteLine($"Data: {BitConverter.ToString(data)}");
        if (data.Length < 8)
            return;

        if (data[2] != 0xdb)
            return;

        var dataByte = data[3];
        var sign = dataByte & 0x80;
        var decimalPoint = (dataByte & 0x70) >> 4;
        var unit = (dataByte & 0x0c) >> 2;

        var valueBytes = new byte[4];

        Array.Copy(data, 4, valueBytes, 0, 3);
        Array.Reverse(valueBytes);

        var valueString = string.Join(string.Empty, valueBytes.Select(x => x.ToString("X2")));
        if (decimalPoint > 0)
            valueString = valueString.Insert(valueString.Length - decimalPoint, ".");

        var value = float.Parse(valueString);
        if (sign == 0x80)
            value *= -1;

        ValueReceived?.Invoke(this, new ValueReceivedEventArgs(value, (byte)unit));
    }

    public class ValueReceivedEventArgs : EventArgs
    {
        public ValueReceivedEventArgs(float value, byte unit)
        {
            Value = value;
            Unit = unit;
        }

        public float Value { get; init; }
        public byte Unit { get; init; }
    }
}