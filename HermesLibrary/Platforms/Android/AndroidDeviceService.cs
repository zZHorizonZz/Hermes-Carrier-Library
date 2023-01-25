using Android.Content;
using QuickStat.Devices.Usb;

namespace QuickStat.Devices;

public class AndroidDeviceService
{
    public static AndroidDeviceService? Current { get; }

    static AndroidDeviceService()
    {
        Current = new AndroidDeviceService();

        var context = Android.App.Application.Context;

        Current.UsbService = new AndroidUsbService(context);
        Current.UsbService.Register();
    }

    public AndroidUsbService? UsbService { get; private set; }
}