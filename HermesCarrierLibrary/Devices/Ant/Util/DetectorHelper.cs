using HermesCarrierLibrary.Devices.Usb;

namespace HermesCarrierLibrary.Devices.Ant.Util;

public static class DetectorHelper
{
    public static bool IsAntDongle(this IUsbDevice device)
    {
        Console.WriteLine($"Dongle: {device.VendorId} {device.ProductId}");
        if (device is not { VendorId: 0x0fcf })
            return false;

        return device.ProductId is >= 0x1003 and <= 0x1009;
    }
}