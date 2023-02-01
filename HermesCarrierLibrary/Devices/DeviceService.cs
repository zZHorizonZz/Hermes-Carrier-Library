using HermesCarrierLibrary.Devices.Ant;
using HermesCarrierLibrary.Devices.Ant.Dongle;
using HermesCarrierLibrary.Devices.Ant.Interfaces;
using HermesCarrierLibrary.Devices.Ant.Util;
using HermesCarrierLibrary.Devices.Shared;
using DeviceType = HermesCarrierLibrary.Devices.Shared.DeviceType;
#if ANDROID
using HermesCarrierLibrary.Platforms.Android.Devices;
#endif

namespace HermesCarrierLibrary.Devices;

public class DeviceService
{
    public DeviceService()
    {
#if ANDROID
        UsbService = AndroidDeviceService.Current?.UsbService;
        UsbService.DevicePermissionGranted += OnConnectSerial;
        UsbService.DeviceDetached += OnDisconnectSerial;
#endif

        if (UsbService is null)
            throw new NullReferenceException("UsbService is null");

        AntService = new AntService(UsbService);
    }

    private readonly WeakEventManager mDeviceConnected = new();
    private readonly WeakEventManager mDeviceDisconnected = new();
    private readonly WeakEventManager mDevicePermissionGranted = new();
    private readonly WeakEventManager mDevicePermissionDenied = new();
    private readonly WeakEventManager mDeviceDetected = new();

    public event EventHandler<DeviceEventArgs> DeviceConnected
    {
        add => mDeviceConnected.AddEventHandler(value);
        remove => mDeviceConnected.RemoveEventHandler(value);
    }

    public event EventHandler<DeviceEventArgs> DeviceDisconnected
    {
        add => mDeviceDisconnected.AddEventHandler(value);
        remove => mDeviceDisconnected.RemoveEventHandler(value);
    }

    public event EventHandler<DeviceEventArgs> DevicePermissionGranted
    {
        add => mDevicePermissionGranted.AddEventHandler(value);
        remove => mDevicePermissionGranted.RemoveEventHandler(value);
    }

    public event EventHandler<DeviceEventArgs> DevicePermissionDenied
    {
        add => mDevicePermissionDenied.AddEventHandler(value);
        remove => mDevicePermissionDenied.RemoveEventHandler(value);
    }

    public event EventHandler<DeviceEventArgs> DeviceDetected
    {
        add => mDeviceDetected.AddEventHandler(value);
        remove => mDeviceDetected.RemoveEventHandler(value);
    }

    public IUsbService? UsbService { get; init; }
    public IAntService AntService { get; init; }

    public void OnConnectSerial(object? sender, UsbActionEventArgs args)
    {
        var deviceType = DeviceType.Usb;
        if (args.Device.IsAntDongle())
        {
            deviceType = DeviceType.Ant;
            var transmitter = new AntDongleTransmitter(args.Device);
            AntService.ConnectTransmitter(transmitter);
        }

        mDeviceConnected.HandleEvent(this,
            new DeviceEventArgs(args.Device, DeviceEventArgs.DeviceAction.DeviceConnected, deviceType),
            nameof(DeviceConnected));
    }

    public void OnDisconnectSerial(object? sender, UsbActionEventArgs args)
    {
        var deviceType = DeviceType.Usb;

        if (args.Device.IsAntDongle())
        {
            deviceType = DeviceType.Ant;
            var transmitter = new AntDongleTransmitter(args.Device);
            AntService.DisconnectTransmitter(transmitter);
        }

        mDeviceDisconnected.HandleEvent(this,
            new DeviceEventArgs(args.Device, DeviceEventArgs.DeviceAction.DeviceDisconnected, deviceType),
            nameof(DeviceDisconnected));
    }
}