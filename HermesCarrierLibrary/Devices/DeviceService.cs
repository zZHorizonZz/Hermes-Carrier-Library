﻿using HermesCarrierLibrary.Devices.Ant;
using HermesCarrierLibrary.Devices.Ant.Dongle;
using HermesCarrierLibrary.Devices.Ant.Interfaces;
using HermesCarrierLibrary.Devices.Ant.Util;
using HermesCarrierLibrary.Devices.Usb;
using Microsoft.Extensions.Logging;
using DeviceType = HermesCarrierLibrary.Devices.Shared.DeviceType;
#if ANDROID
using HermesCarrierLibrary.Platforms.Android.Devices;
#endif

namespace HermesCarrierLibrary.Devices;

public class DeviceService
{
    public static readonly ILogger Logger = new LoggerFactory().CreateLogger<DeviceService>();

    private readonly WeakEventManager mDeviceConnected = new();
    private readonly WeakEventManager mDeviceDetected = new();
    private readonly WeakEventManager mDeviceDisconnected = new();
    private readonly WeakEventManager mDevicePermissionDenied = new();
    private readonly WeakEventManager mDevicePermissionGranted = new();

    public DeviceService()
    {
        Logger.LogInformation("Starting {0}...", nameof(DeviceService));
#if ANDROID
        Logger.LogInformation("Detected Android platform. Initializing {0}...", nameof(AndroidDeviceService));
        UsbService = AndroidDeviceService.Current?.UsbService;
        UsbService.DeviceAttached += OnConnect;
        UsbService.DeviceDetached += OnDisconnect;

        if (UsbService is null)
            throw new NullReferenceException("UsbService is null");

        AntService = new AntService(UsbService);

        Logger.LogInformation("{0} initialized.", nameof(AndroidDeviceService));
#elif IOS
        Logger.LogInformation("Detected iOS platform. Initializing ...");
        Logger.LogInformation("iOS Currently not supported.");
#elif WINDOWS
        Logger.LogInformation("Detected Windows platform. Initializing ...");
        Logger.LogInformation("Windows Currently not supported.");
#endif
    }

    public IUsbService UsbService { get; init; }
    public IAntService AntService { get; init; }

    public IEnumerable<IUsbDevice> UsbDevices => UsbService.Devices;

    public IEnumerable<IAntTransmitter> AntTransmitters => AntService.Transmitters.Values;

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

    private void OnConnect(object? sender, UsbActionEventArgs args)
    {
        var deviceType = DeviceType.Usb;
        if (args.Device.IsAntDongle())
        {
            deviceType = DeviceType.Ant;
            var transmitter = new AntDongleTransmitter(args.Device);
            AntService.ConnectTransmitter(transmitter);
            Logger.LogInformation("Detected ANT device: {0}", args.Device);
        }

        mDeviceConnected.HandleEvent(this,
            new DeviceEventArgs(args.Device, DeviceEventArgs.DeviceAction.DeviceConnected, deviceType),
            nameof(DeviceConnected));

        Logger.LogInformation("Connected to device: {0}", args.Device);
    }

    private void OnDisconnect(object? sender, UsbActionEventArgs args)
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

        Logger.LogInformation("Disconnected from device: {0}", args.Device);
    }
}