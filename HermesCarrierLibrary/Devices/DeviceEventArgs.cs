using HermesCarrierLibrary.Devices.Shared;
using DeviceType = HermesCarrierLibrary.Devices.Shared.DeviceType;

namespace HermesCarrierLibrary.Devices;

public class DeviceEventArgs : System.EventArgs
{
    public DeviceEventArgs(ISerial serial, DeviceAction action, DeviceType deviceType)
    {
        Serial = serial;
        Action = action;
        DeviceType = deviceType;
    }

    public ISerial Serial { get; }

    public DeviceAction Action { get; }

    public DeviceType DeviceType { get; }

    public enum DeviceAction
    {
        DeviceConnected = 0,
        DeviceDisconnected = 1,
        DevicePermissionGranted = 2,
        DevicePermissionDenied = 3,
        DeviceDetected = 4
    }
}