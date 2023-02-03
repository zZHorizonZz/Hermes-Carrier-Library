using Android.Content;
using Android.Hardware.Usb;

namespace HermesCarrierLibrary.Platforms.Android.Devices;

public class UsbBroadcastReceiver : BroadcastReceiver
{
    private readonly DroidUsbService mService;

    public UsbBroadcastReceiver(DroidUsbService service)
    {
        mService = service;
    }


    /// <inheritdoc />
    public override void OnReceive(Context? context, Intent? intent)
    {
        var action = intent?.Action;
        switch (action)
        {
            case null:
                return;
            case DroidUsbDevice.ActionUsbPermission:
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