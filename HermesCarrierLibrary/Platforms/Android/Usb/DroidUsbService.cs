using Android.Content;
using Android.Hardware.Usb;
using HermesCarrierLibrary.Devices.Usb;

namespace HermesCarrierLibrary.Platforms.Android.Devices;

public class DroidUsbService : IUsbService
{
    private readonly Context mContext;
    private readonly WeakEventManager mDeviceAttached = new();
    private readonly WeakEventManager mDeviceDetached = new();
    private readonly WeakEventManager mDevicePermissionDenied = new();

    private readonly WeakEventManager mDevicePermissionGranted = new();
    private readonly IDictionary<int, IUsbDevice> mDevices = new Dictionary<int, IUsbDevice>();
    private readonly UsbBroadcastReceiver mUsbBroadcastReceiver;
    private readonly UsbManager mUsbManager;

    public DroidUsbService(Context context)
    {
        mContext = context;
        mUsbManager = (UsbManager)context.GetSystemService(Context.UsbService);
        mUsbBroadcastReceiver = new UsbBroadcastReceiver(this);
    }

    public event EventHandler<UsbActionEventArgs> DevicePermissionGranted
    {
        add => mDevicePermissionGranted.AddEventHandler(value);
        remove => mDevicePermissionGranted.RemoveEventHandler(value);
    }

    public event EventHandler<UsbActionEventArgs> DevicePermissionDenied
    {
        add => mDevicePermissionDenied.AddEventHandler(value);
        remove => mDevicePermissionDenied.RemoveEventHandler(value);
    }

    public event EventHandler<UsbActionEventArgs> DeviceAttached
    {
        add => mDeviceAttached.AddEventHandler(value);
        remove => mDeviceAttached.RemoveEventHandler(value);
    }

    public event EventHandler<UsbActionEventArgs> DeviceDetached
    {
        add => mDeviceDetached.AddEventHandler(value);
        remove => mDeviceDetached.RemoveEventHandler(value);
    }

    /// <inheritdoc />
    public IEnumerable<IUsbDevice> GetDevices()
    {
        var knownDevices = mDevices.Values;
        var usbDevices = mUsbManager.DeviceList.Values;

        foreach (var usbDevice in usbDevices)
        {
            if (knownDevices.Any(d => d.DeviceId == usbDevice.DeviceId))
                continue;

            var device = new DroidUsbDevice(usbDevice, mContext, mUsbManager);
            mDevices.Add(usbDevice.DeviceId, device);
        }

        return mDevices.Values;
    }

    public void Register()
    {
        var filter = new IntentFilter();
        filter.AddAction(UsbManager.ActionUsbDeviceAttached);
        filter.AddAction(UsbManager.ActionUsbDeviceDetached);
        filter.AddAction(DroidUsbDevice.ActionUsbPermission);
        mContext.RegisterReceiver(mUsbBroadcastReceiver, filter);
    }

    public void Unregister()
    {
        mContext.UnregisterReceiver(mUsbBroadcastReceiver);
    }

    public void OnDevicePermissionGranted(UsbDevice device)
    {
        var serial = mDevices[device.DeviceId];
        if (serial is DroidUsbDevice droidUsbDevice)
            droidUsbDevice.PermissionResult(true);

        mDevicePermissionGranted.HandleEvent(this,
            new UsbActionEventArgs(UsbActionEventArgs.UsbAction.DevicePermissionGranted, serial),
            nameof(DevicePermissionGranted));
    }

    public void OnDevicePermissionDenied(UsbDevice device)
    {
        var serial = mDevices[device.DeviceId];
        if (serial is DroidUsbDevice droidUsbDevice)
            droidUsbDevice.PermissionResult(false);

        mDevicePermissionDenied.HandleEvent(this,
            new UsbActionEventArgs(UsbActionEventArgs.UsbAction.DevicePermissionDenied, serial),
            nameof(DevicePermissionDenied));
    }

    public void OnDeviceAttached(UsbDevice device)
    {
        var serial = new DroidUsbDevice(device, mContext, mUsbManager);
        mDevices.Add(device.DeviceId, serial);

        mDeviceAttached.HandleEvent(this,
            new UsbActionEventArgs(UsbActionEventArgs.UsbAction.DeviceAttached, serial),
            nameof(DeviceAttached));
    }

    public void OnDeviceDetached(UsbDevice device)
    {
        var serial = mDevices[device.DeviceId];
        mDevices.Remove(device.DeviceId);

        mDeviceDetached.HandleEvent(this,
            new UsbActionEventArgs(UsbActionEventArgs.UsbAction.DeviceDetached, serial),
            nameof(DeviceDetached));
    }
}