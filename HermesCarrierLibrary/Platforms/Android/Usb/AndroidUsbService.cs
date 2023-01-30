using Android.App;
using Android.Content;
using Android.Hardware.Usb;
using HermesCarrierLibrary.Devices.Shared;

namespace HermesCarrierLibrary.Platforms.Android.Devices;

public class AndroidUsbService : IUsbService
{
    public const string ActionUsbPermission = "cz.palstat.quickstat.USB_PERMISSION";

    private readonly Context mContext;
    private readonly WeakEventManager mDeviceAttached = new();
    private readonly WeakEventManager mDeviceDetached = new();
    private readonly WeakEventManager mDevicePermissionDenied = new();

    private readonly WeakEventManager mDevicePermissionGranted = new();
    private readonly IDictionary<int, UsbSerial> mDevices = new Dictionary<int, UsbSerial>();
    private readonly UsbBroadcastReceiver mUsbBroadcastReceiver;
    private readonly UsbManager mUsbManager;

    public AndroidUsbService(Context context)
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
    public IEnumerable<ISerial> GetDevices()
    {
        var devices = mUsbManager.DeviceList.Values;

        foreach (var device in devices)
            if (mDevices.TryGetValue(device.DeviceId, out var value))
            {
                yield return value;
            }
            else
            {
                var serial = new UsbSerial(device);
                mDevices.Add(device.DeviceId, serial);
                yield return serial;
            }
    }

    public void Register()
    {
        var filter = new IntentFilter();
        filter.AddAction(UsbManager.ActionUsbDeviceAttached);
        filter.AddAction(UsbManager.ActionUsbDeviceDetached);
        filter.AddAction(ActionUsbPermission);
        mContext.RegisterReceiver(mUsbBroadcastReceiver, filter);
    }

    public void Unregister()
    {
        mContext.UnregisterReceiver(mUsbBroadcastReceiver);
    }

    public void OnDevicePermissionGranted(UsbDevice device)
    {
        var serial = mDevices[device.DeviceId];
        serial.Open(mContext);

        mDevicePermissionGranted.HandleEvent(this,
            new UsbActionEventArgs(UsbActionEventArgs.UsbAction.DevicePermissionGranted, serial),
            nameof(DevicePermissionGranted));
    }

    public void OnDevicePermissionDenied(UsbDevice device)
    {
        var serial = mDevices[device.DeviceId];

        mDevicePermissionDenied.HandleEvent(this,
            new UsbActionEventArgs(UsbActionEventArgs.UsbAction.DevicePermissionDenied, serial),
            nameof(DevicePermissionDenied));
    }

    public void OnDeviceAttached(UsbDevice device)
    {
        Console.WriteLine($"Device attached: {device.DeviceName} {device.DeviceId}");
        var serial = new UsbSerial(device);
        mDevices.Add(device.DeviceId, serial);

        mDeviceAttached.HandleEvent(this,
            new UsbActionEventArgs(UsbActionEventArgs.UsbAction.DeviceAttached, serial),
            nameof(DeviceAttached));

        if (mUsbManager.HasPermission(device))
            OnDevicePermissionGranted(device);
        else
            RequestPermission(device);
    }

    public void OnDeviceDetached(UsbDevice device)
    {
        Console.WriteLine($"Device detached: {device.DeviceName} {device.DeviceId}");
        var serial = mDevices[device.DeviceId];
        mDevices.Remove(device.DeviceId);

        mDeviceDetached.HandleEvent(this,
            new UsbActionEventArgs(UsbActionEventArgs.UsbAction.DeviceDetached, serial),
            nameof(DeviceDetached));
    }

    public void RequestPermission(UsbDevice device)
    {
        var pendingIntent = PendingIntent.GetBroadcast(mContext,
            0,
            new Intent(ActionUsbPermission),
            PendingIntentFlags.Mutable);
        mUsbManager.RequestPermission(device, pendingIntent);
    }
}