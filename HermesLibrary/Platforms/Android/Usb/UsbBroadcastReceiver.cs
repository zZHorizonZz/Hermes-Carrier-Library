using Android.Content;
using Android.Hardware.Usb;
using HermesLibrary.Devices.Ant.Dongle;
using QuickStat.Devices.Usb;

namespace QuickStat.Platforms.Android.Devices.Ant.Dongle;

public class UsbBroadcastReceiver : BroadcastReceiver
{
    private readonly AndroidUsbService mService;

    public UsbBroadcastReceiver(AndroidUsbService service)
    {
        mService = service;
    }


    /// <inheritdoc />
    public override void OnReceive(Context? context, Intent? intent)
    {
        var action = intent?.Action;
        Console.WriteLine($"DongleBroadcastReceiver.OnReceive: {action}");
        switch (action)
        {
            case null:
                return;
            case DongleAntService.ActionUsbPermission:
            {
                if (intent?.GetBooleanExtra("permission", false) == true)
                {
                    var device = (UsbDevice?)intent?.GetParcelableExtra(UsbManager.ExtraDevice);
                    if (device != null) mService?.OnDevicePermissionGranted(device);
                }
                else
                {
                    var device = (UsbDevice?)intent?.GetParcelableExtra(UsbManager.ExtraDevice);
                    if (device != null) mService?.OnDevicePermissionDenied(device);
                }

                break;
            }

            case UsbManager.ActionUsbDeviceDetached:
            {
                if (intent?.GetParcelableExtra(UsbManager.ExtraDevice) is UsbDevice usbDevice)
                    mService?.OnDeviceDetached(usbDevice);

                break;
            }

            case UsbManager.ActionUsbDeviceAttached:
            {
                if (intent?.GetParcelableExtra(UsbManager.ExtraDevice) is UsbDevice usbDevice)
                    mService?.OnDeviceAttached(usbDevice);

                break;
            }
        }
    }
}