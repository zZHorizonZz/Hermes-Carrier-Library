using Application = Android.App.Application;

namespace HermesCarrierLibrary.Platforms.Android.Devices;

public class AndroidDeviceService
{
    static AndroidDeviceService()
    {
        Current = new AndroidDeviceService();

        var context = Application.Context;

        Current.UsbService = new AndroidUsbService(context);
        Current.UsbService.Register();
    }

    public static AndroidDeviceService? Current { get; }

    public AndroidUsbService? UsbService { get; private set; }
}