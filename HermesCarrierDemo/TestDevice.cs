using HermesCarrierLibrary.Devices.Ant;
using HermesCarrierLibrary.Devices.Ant.Channel;
using HermesCarrierLibrary.Devices.Ant.Enum;
using HermesCarrierLibrary.Devices.Ant.Interfaces;
using HermesCarrierLibrary.Devices.Ant.Messages.Client;
using HermesCarrierLibrary.Devices.Ant.Messages.Device;
using HermesCarrierLibrary.Devices.Ant.Messages.Shared;
using Microsoft.Extensions.Logging;
using ChannelIdMessage = HermesCarrierLibrary.Devices.Ant.Messages.Client.ChannelIdMessage;

namespace HermesCarrierDemo;

public class TestDevice
{
    /// <summary>
    ///     Gets the unique device number assigned to the ANT transmitter.
    /// </summary>
    public ushort DeviceNumber { get; init; } = 0x7d0;

    /// <summary>
    ///     Gets a value indicating whether the ANT transmitter is configured as a pairing device.
    /// </summary>
    public bool IsPairing { get; init; }

    /// <summary>
    ///     Gets the type of device the ANT transmitter is communicating with.
    /// </summary>
    public byte DeviceType { get; init; } = 0x05;

    /// <summary>
    ///     Gets the type of transmission being used by the ANT transmitter, such as asynchronous or synchronous.
    /// </summary>
    public byte TransmissionType { get; init; } = 0x01;

    public event EventHandler<ValueReceivedEventArgs> ValueReceived;

    private IAntTransmitter mTransmitter;
    private Timer mHeartbeatTimer;

    public async Task Open(IAntTransmitter transmitter, IAntChannel channel)
    {
        mTransmitter = transmitter;

        Console.WriteLine("Opening ANT transmitter...");
        if (!transmitter.IsConnected)
            await transmitter.OpenAsync();

        Console.WriteLine("Resetting ANT transmitter...");
        await transmitter.SendMessageAsync(new ResetSystemMessage());
        Thread.Sleep(500);
        await transmitter.SendMessageAsync(new SetNetworkKeyMessage(1,
            new byte[] { 0xF9, 0xED, 0x22, 0xB8, 0xFD, 0x56, 0x67, 0xCD }));

        Console.WriteLine("Configuring ANT channel...");
        await channel.AssignChannel(transmitter);

        var result = (await channel.AwaitMessageOfTypeAsync<EventResponseMessage>(new ChannelIdMessage(DeviceNumber,
            IsPairing,
            DeviceType,
            TransmissionType))).Type;

        Console.WriteLine($"Channel id set to {result}");
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
        Thread.Sleep(250);
        StartHeartbeat(channel);
    }

    public void StartHeartbeat(IAntChannel channel)
    {
        mHeartbeatTimer = new Timer(async _ => await Heartbeat(channel), null, 0, 5000);
    }

    public async Task Heartbeat(IAntChannel channel)
    {
        if (!mTransmitter.IsConnected)
        {
            await mHeartbeatTimer.DisposeAsync();
            return;
        }

        var response = await channel.AwaitMessageOfTypeAsync<EventResponseMessage>(new BroadcastDataMessage(new byte[]
            { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }));

        if (response.Type != EventResponseType.RESPONSE_NO_ERROR)
        {
            Console.WriteLine($"Failed to send heartbeat {response.Type}");
            if (mTransmitter.IsChannelOpen(channel.Number))
                await mTransmitter.CloseChannelAsync(channel);
        }
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