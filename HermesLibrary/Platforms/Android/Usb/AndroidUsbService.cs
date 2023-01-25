using Android.App;
using Android.Content;
using Android.Hardware.Usb;
using QuickStat.Platforms.Android.Devices.Ant.Dongle;

namespace QuickStat.Devices.Usb;

public class AndroidUsbService : IUsbService
{
    public const string ActionUsbPermission = "cz.palstat.quickstat.USB_PERMISSION";

    private readonly Context mContext;
    private readonly WeakEventManager mDeviceAttached = new();
    private readonly WeakEventManager mDeviceDetached = new();
    private readonly WeakEventManager mDevicePermissionDenied = new();

    private readonly WeakEventManager mDevicePermissionGranted = new();
    private readonly IDictionary<UsbDevice, UsbSerial> mDevices = new Dictionary<UsbDevice, UsbSerial>();
    private readonly UsbBroadcastReceiver mUsbBroadcastReceiver;
    private readonly UsbManager mUsbManager;

    public AndroidUsbService(Context context)
    {
        mContext = context;
        mUsbManager = (UsbManager)context.GetSystemService(Context.UsbService);
        mUsbBroadcastReceiver = new UsbBroadcastReceiver(this);
    }

    public event EventHandler DevicePermissionGranted
    {
        add => mDevicePermissionGranted.AddEventHandler(value);
        remove => mDevicePermissionGranted.RemoveEventHandler(value);
    }

    public event EventHandler DevicePermissionDenied
    {
        add => mDevicePermissionDenied.AddEventHandler(value);
        remove => mDevicePermissionDenied.RemoveEventHandler(value);
    }

    public event EventHandler DeviceAttached
    {
        add => mDeviceAttached.AddEventHandler(value);
        remove => mDeviceAttached.RemoveEventHandler(value);
    }

    public event EventHandler DeviceDetached
    {
        add => mDeviceDetached.AddEventHandler(value);
        remove => mDeviceDetached.RemoveEventHandler(value);
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
        var serial = mDevices[device];

        mDevicePermissionGranted.HandleEvent(this,
            new UsbActionEventArgs(UsbActionEventArgs.UsbAction.DevicePermissionGranted, serial),
            nameof(DevicePermissionGranted));
    }

    public void OnDevicePermissionDenied(UsbDevice device)
    {
        var serial = mDevices[device];

        mDevicePermissionDenied.HandleEvent(this,
            new UsbActionEventArgs(UsbActionEventArgs.UsbAction.DevicePermissionDenied, serial),
            nameof(DevicePermissionDenied));
    }

    public void OnDeviceAttached(UsbDevice device)
    {
        var serial = new UsbSerial(device);
        mDevices.Add(device, serial);

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
        var serial = mDevices[device];
        mDevices.Remove(device);

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