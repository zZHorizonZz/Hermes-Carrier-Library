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
        new UnAssignChannelMessage(),
        new AssignChannelMessage(),
        new ChannelIdMessage(),
        new ChannelPeriodMessage(),
        new SearchTimeoutMessage(),
        new ChannelRfFrequencyMessage(),
        new SetNetworkKeyMessage(),
        new TransmitPowerMessage(),
        new SearchWaveformMessage(),
        new AddChannelIdToListMessage(),
        new AddEncryptionIdToListMessage(),
        new ConfigIdListMessage(),
        new ConfigEncryptionIdListMessage(),
        new SetChannelTransmitPowerMessage(),
        new LowPrioritySearchTimeoutMessage(),
        new SerialNumberSetChannelIdMessage(),
        new EnableExtRxMessagesMessage(),
        new EnableLedMessage(),
        new EnableCrystalMessage(),
        new LibConfigMessage(),
        new FrequencyAgilityMessage(),
        new ProximitySearchMessage(),
        new ConfigureEventBufferMessage(),
        new ChannelSearchPriorityMessage(),
        new Set128BitNetworkKeyMessage(),
        new HighDutySearchMessage(),
        new ConfigureAdvancedBurstMessage(),
        new ConfigureEventFilterMessage(),
        new ConfigureSelectiveDataUpdatesMessage(),
        new SetSelectiveDataUpdateMaskMessage(),
        new ConfigureUserNvmMessage(),
        new EnableSingleChannelEncryptionMessage(),
        new SetEncryptionKeyMessage(),
        new SetEncryptionInfoMessage(),
        new ChannelSearchSharingMessage(),
        new LoadStoreEncryptionKeyMessage(),
        new SetUsbDescriptorStringMessage(),
        new ResetSystemMessage(),
        new OpenChannelMessage(),
        new CloseChannelMessage(),
        new RequestMessage(),
        new OpenRxScanMode(),
        new SleepMessage(),
        new CwInitMessage(),
        new CwTestMessage()
    };

    public static readonly IAntMessage[] DeviceBoundMessages =
    {
        new StartUpMessage(),
        new SerialErrorMessage(),
        new EventResponseMessage(),
        new ChannelStatusMessage(),
        new ChannelIdMessage(),
        new AntVersionMessage(),
        new CapabilitiesMessage(),
        new SerialNumberMessage(),
        new EventBufferConfigurationMessage(),
        //new AdvancedBurstCapabilitiesMessage(), TODO: Rework this message there are two states 0 and 1 where 0 is short and 1 is long
        new EventFilterMessage(),
        new SelectiveDataUpdateMaskSettingMessage(),
        new UserNvmMessage(),
        new EncryptionModeParametersMessage()
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