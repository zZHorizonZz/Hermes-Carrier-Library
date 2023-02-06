using HermesCarrierLibrary.Devices.Usb;
using DeviceType = HermesCarrierLibrary.Devices.Shared.DeviceType;

namespace HermesCarrierLibrary.Devices;

public class DeviceEventArgs : EventArgs
{
    public enum DeviceAction
    {
        DeviceConnected = 0,
        DeviceDisconnected = 1,
        DevicePermissionGranted = 2,
        DevicePermissionDenied = 3,
        DeviceDetected = 4
    }

    public DeviceEventArgs(IUsbDevice serial, DeviceAction action, DeviceType deviceType)
    {
        Serial = serial;
        Action = action;
        DeviceType = deviceType;
    }

    public IUsbDevice Serial { get; }

    public DeviceAction Action { get; }

    public DeviceType DeviceType { get; }
}