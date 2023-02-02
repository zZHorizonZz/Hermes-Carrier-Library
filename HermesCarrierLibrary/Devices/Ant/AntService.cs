using HermesCarrierLibrary.Devices.Ant.Dongle;
using HermesCarrierLibrary.Devices.Ant.Interfaces;
using HermesCarrierLibrary.Devices.Ant.Messages.Client;
using HermesCarrierLibrary.Devices.Ant.Messages.Device;
using HermesCarrierLibrary.Devices.Ant.Messages.Shared;
using HermesCarrierLibrary.Devices.Ant.Util;
using HermesCarrierLibrary.Devices.Usb;
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
        new CwTestMessage(),
        new AcknowledgedDataMessage(),
        new AdvancedBurstDataMessage(),
        new BroadcastDataMessage(),
        new BurstTransferDataMessage()
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
        new EncryptionModeParametersMessage(),
        new AcknowledgedDataMessage(),
        new AdvancedBurstDataMessage(),
        new BroadcastDataMessage(),
        new BurstTransferDataMessage()
    };

    private readonly IUsbService mUsbService;

    public AntService(IUsbService usbService)
    {
        mUsbService = usbService;
    }

    /// <inheritdoc />
    public IDictionary<int, IAntTransmitter> Transmitters { get; } = new Dictionary<int, IAntTransmitter>();

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
        Transmitters.Add(transmitter.GetHashCode(), transmitter);
    }

    /// <inheritdoc />
    public void DisconnectTransmitter(IAntTransmitter transmitter)
    {
        Transmitters.Remove(transmitter.GetHashCode());
    }
}