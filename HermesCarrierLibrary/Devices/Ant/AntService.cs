using HermesCarrierLibrary.Devices.Ant.Dongle;
using HermesCarrierLibrary.Devices.Ant.EventArgs;
using HermesCarrierLibrary.Devices.Ant.Interfaces;
using HermesCarrierLibrary.Devices.Ant.Messages.Client;
using HermesCarrierLibrary.Devices.Ant.Messages.Device;
using HermesCarrierLibrary.Devices.Ant.Util;
using HermesCarrierLibrary.Devices.Shared;
using ChannelIdMessage = HermesCarrierLibrary.Devices.Ant.Messages.Client.ChannelIdMessage;

namespace HermesCarrierLibrary.Devices.Ant;

public class AntService : IAntService
{
    public static readonly IAntMessage[] ClientBoundMessages =
    {
        new AcknowledgedDataMessage(),
        new AssignChannelMessage(),
        new BroadcastDataMessage(),
        new ChannelPeriodMessage(),
        new ChannelRfFrequencyMessage(),
        new CloseChannelMessage(),
        new OpenChannelMessage(),
        new OpenRxScanMode(),
        new RequestMessage(),
        new ResetSystemMessage(),
        new ChannelIdMessage(),
        new SetNetworkKeyMessage(),
        new UnAssignChannelMessage(),
    };

    public static readonly IAntMessage[] DeviceBoundMessages =
    {
        new AcknowledgedDataMessage(),
        new AntVersionMessage(),
        new EventResponseMessage(),
        new StartUpMessage(),
    };

    private readonly WeakEventManager mTransmitterStatusChangedEventManager = new();

    private readonly IUsbService mUsbService;

    public AntService(IUsbService usbService)
    {
        mUsbService = usbService;
    }

    /// <inheritdoc />
    public IAntTransmitter CurrentTransmitter { get; set; }

    /// <inheritdoc />
    public event EventHandler<AntTransmitterStatusChangedEventArgs> TransmitterStatusChanged
    {
        add => mTransmitterStatusChangedEventManager.AddEventHandler(value);
        remove => mTransmitterStatusChangedEventManager.RemoveEventHandler(value);
    }

    /// <inheritdoc />
    public IAntTransmitter[] DetectTransmitters()
    {
        return (from device in mUsbService.GetDevices()
            where device.IsAntDongle()
            select new AntDongleTransmitter(device)).Cast<IAntTransmitter>().ToArray();
    }

    /// <inheritdoc />
    public void ConnectTransmitter(IAntTransmitter transmitter)
    {
        Console.WriteLine($"ConnectTransmitter: {transmitter}");
        if (CurrentTransmitter != null)
        {
            //TODO Close current transmitter
        }

        if (!transmitter.IsConnected)
            throw new Exception("Transmitter is not connected");

        CurrentTransmitter = transmitter;
        mTransmitterStatusChangedEventManager.HandleEvent(this, new AntTransmitterStatusChangedEventArgs(transmitter),
            nameof(TransmitterStatusChanged));
    }

    /// <inheritdoc />
    public void DisconnectTransmitter(IAntTransmitter transmitter)
    {
        Console.WriteLine($"DisconnectTransmitter: {transmitter}");
        CurrentTransmitter = null;
        mTransmitterStatusChangedEventManager.HandleEvent(this, new AntTransmitterStatusChangedEventArgs(transmitter),
            nameof(TransmitterStatusChanged));
    }
}