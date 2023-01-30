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
    private IAntTransmitter mTransmitter;

    [ObservableProperty] private string _status = "Disconnected";

    [ObservableProperty] private Color _statusColor = Color.FromArgb("#FF0000");

    [ObservableProperty] private string _deviceNumber = "2000";

    [ObservableProperty] private string _deviceType = "5";

    [ObservableProperty] private string _transmissionType = "1";

    private IAntChannel mChannel;

    public event EventHandler<TestDevice.ValueReceivedEventArgs> ValueReceived;

    public AntDongleViewModel()
    {
        mDeviceService.AntService.TransmitterStatusChanged += OnTransmitterStatusChanged;
    }

    [RelayCommand]
    public async Task Connect()
    {
        if (mChannel is not null)
            await mChannel.Close();

        mChannel = new Channel(0, 1, ChannelType.TransmitChannel, ExtendedAssignmentType.UNKNOWN, 0x2000,
            0x03);

        var testDevice = new TestDevice()
        {
            DeviceNumber = ushort.Parse(DeviceNumber),
            DeviceType = byte.Parse(DeviceType),
            TransmissionType = byte.Parse(TransmissionType)
        };

        testDevice.ValueReceived += OnValueReceived;
        await testDevice.Open(mTransmitter, mChannel);
    }

    public void OnValueReceived(object sender, TestDevice.ValueReceivedEventArgs e)
    {
        ValueReceived?.Invoke(sender, e);
    }

    private void OnTransmitterStatusChanged(object sender, AntTransmitterStatusChangedEventArgs e)
    {
        Console.WriteLine($"Transmitter status changed: {e.Transmitter.IsConnected}");
        Status = e.Transmitter.IsConnected ? "Connected" : "Disconnected";
        StatusColor = e.Transmitter.IsConnected ? Color.FromArgb("#00FF00") : Color.FromArgb("#FF0000");

        if (e.Transmitter.IsConnected)
        {
            mTransmitter = e.Transmitter;
        }
    }
}