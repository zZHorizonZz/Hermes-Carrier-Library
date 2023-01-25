#if ANDROID
using QuickStat.Devices;
using QuickStat.Platforms.Android.Devices.Ant.Dongle;
#endif

namespace HermesLibrary.Devices;

public class DeviceService
{
    public DeviceService()
    {
#if ANDROID
        UsbService = AndroidDeviceService.Current?.UsbService;
#endif
    }

    public IUsbService? UsbService { get; init; }
    public IAntService AntService { get; init; }
}