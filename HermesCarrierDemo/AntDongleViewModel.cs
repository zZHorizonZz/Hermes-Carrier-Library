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

    [ObservableProperty] private bool _Pairing = false;

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
        Console.WriteLine("Connecting to ANT transmitter...");
        if (mChannel is not null)
            await mTransmitter.CloseChannelAsync(mChannel);

        Console.WriteLine("Opening ANT transmitter...");
        mChannel = new Channel(
            0,
            1,
            ChannelType.TransmitChannel,
            ExtendedAssignmentType.UNKNOWN,
            0x2000,
            0x03);

        Console.WriteLine("Opening ANT channel...");
        var testDevice = new TestDevice
        {
            DeviceNumber = ushort.Parse(DeviceNumber),
            DeviceType = byte.Parse(DeviceType),
            TransmissionType = byte.Parse(TransmissionType),
            IsPairing = Pairing
        };

        Console.WriteLine("Opening ANT device 1...");
        testDevice.ValueReceived += OnValueReceived;
        Console.WriteLine("Opening ANT device 2...");
        await testDevice.Open(mTransmitter, mChannel);

        Console.WriteLine("Opening ANT device 3...");
        AntVersion = mTransmitter.AntVersion;
        SerialNumber = mTransmitter.SerialNumber;
        Capabilities = mTransmitter.Capabilities.Select(capability => Enum.GetName(capability) ?? "Unknown" + $" ({(byte)capability:X2})")
            .Aggregate((a, b) => $"{a}\n{b}");
    }

    [RelayCommand]
    public async Task Disconnect()
    {
        await mTransmitter.CloseChannelAsync(mChannel);
        mChannel = null;
    }

    public void OnValueReceived(object sender, TestDevice.ValueReceivedEventArgs e)
    {
        ValueReceived?.Invoke(sender, e);
    }

    private void OnDeviceConnected(object sender, DeviceEventArgs e)
    {
        var transmitter = mDeviceService.AntService.Transmitters[e.Serial.DeviceId ^ 31];
        mTransmitter = transmitter;
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