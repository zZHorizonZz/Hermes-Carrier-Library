namespace HermesCarrierLibrary.Devices.Usb;

public class UsbActionEventArgs : EventArgs
{
    public enum UsbAction
    {
        DeviceAttached = 0,
        DeviceDetached = 1,
        DevicePermissionGranted = 2,
        DevicePermissionDenied = 3
    }

    public UsbActionEventArgs(UsbAction action, IUsbDevice device)
    {
        Action = action;
        Device = device;
    }

    public UsbAction Action { get; }

    public IUsbDevice Device { get; }
}