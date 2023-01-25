namespace HermesLibrary.Devices.Shared;

public interface IUsbService
{
    event EventHandler DevicePermissionGranted;
    event EventHandler DevicePermissionDenied;
    event EventHandler DeviceAttached;
    event EventHandler DeviceDetached;
}