using HermesCarrierLibrary.Devices.Shared;

namespace HermesCarrierLibrary.Devices.Usb;

public interface IUsbService
{
    event EventHandler<UsbActionEventArgs> DevicePermissionGranted;
    event EventHandler<UsbActionEventArgs> DevicePermissionDenied;
    event EventHandler<UsbActionEventArgs> DeviceAttached;
    event EventHandler<UsbActionEventArgs> DeviceDetached;

    IEnumerable<ISerial> GetDevices();
}