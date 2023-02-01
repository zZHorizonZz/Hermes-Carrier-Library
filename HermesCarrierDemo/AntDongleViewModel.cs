using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HermesCarrierLibrary.Devices;
using HermesCarrierLibrary.Devices.Ant.Channel;
using HermesCarrierLibrary.Devices.Ant.Enum;
using HermesCarrierLibrary.Devices.Ant.EventArgs;
using HermesCarrierLibrary.Devices.Ant.Interfaces;

namespace HermesCarrierDemo;

public partial class AntDongleViewModel : ObservableObject
{
    private readonly DeviceService mDeviceService = new();

    [ObservableProperty] private string _deviceNumber = "2000";

    [ObservableProperty] private string _deviceType = "5";

    [ObservableProperty] private string _status = "Disconnected";

    [ObservableProperty] private Color _statusColor = Color.FromArgb("#FF0000");

    [ObservableProperty] private string _transmissionType = "1";

    [ObservableProperty] private string _antVersion = "Unknown";

    [ObservableProperty] private string _serialNumber = "Unknown";

    [ObservableProperty] private string _capabilities = "Unknown";

    private IAntChannel mChannel;
    private IAntTransmitter mTransmitter;

    public AntDongleViewModel()
    {
        mDeviceService.DeviceConnected += OnDeviceConnected;
        mDeviceService.DeviceDisconnected += OnDeviceDisconnected;
    }

    public event EventHandler<TestDevice.ValueReceivedEventArgs> ValueReceived;

    [RelayCommand]
    public async Task Connect()
    {
        if (mChannel is not null)
            await mChannel.Close();

        mChannel = new Channel(0, 1, ChannelType.TransmitChannel, ExtendedAssignmentType.UNKNOWN, 0x2000,
            0x03);

        var testDevice = new TestDevice
        {
            DeviceNumber = ushort.Parse(DeviceNumber),
            DeviceType = byte.Parse(DeviceType),
            TransmissionType = byte.Parse(TransmissionType)
        };

        testDevice.ValueReceived += OnValueReceived;
        await testDevice.Open(mTransmitter, mChannel);

        AntVersion = mTransmitter.AntVersion;
        SerialNumber = mTransmitter.SerialNumber;
        Capabilities = mTransmitter.Capabilities
            .Select(capability => Enum.GetName(capability) ?? "Unknown" + $" ({(byte)capability:X2})")
            .Aggregate((a, b) => $"{a}\n{b}");
    }

    public void OnValueReceived(object sender, TestDevice.ValueReceivedEventArgs e)
    {
        ValueReceived?.Invoke(sender, e);
    }

    private void OnDeviceConnected(object sender, DeviceEventArgs e)
    {
        var transmitter = mDeviceService.AntService.Transmitters[e.Serial.VendorId ^ e.Serial.ProductId];
        Status = transmitter.IsConnected ? "Connected" : "Disconnected";
        StatusColor = transmitter.IsConnected ? Color.FromArgb("#00FF00") : Color.FromArgb("#FF0000");

        if (transmitter.IsConnected) mTransmitter = transmitter;
    }

    private void OnDeviceDisconnected(object sender, DeviceEventArgs e)
    {
        Reset();
    }

    private void Reset()
    {
        Status = "Disconnected";
        StatusColor = Color.FromArgb("#FF0000");
        AntVersion = "Unknown";
        SerialNumber = "Unknown";
        Capabilities = "Unknown";
    }
}