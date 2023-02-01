namespace HermesCarrierLibrary.Devices.Shared;

public class UsbActionEventArgs : EventArgs
{
    public enum UsbAction
    {
        DeviceAttached = 0,
        DeviceDetached = 1,
        DevicePermissionGranted = 2,
        DevicePermissionDenied = 3
    }

    public UsbActionEventArgs(UsbAction action, ISerial device)
    {
        Action = action;
        Device = device;
    }

    public UsbAction Action { get; }

    public ISerial Device { get; }
}