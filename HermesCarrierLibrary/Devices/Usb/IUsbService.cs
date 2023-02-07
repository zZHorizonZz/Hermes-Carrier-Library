namespace HermesCarrierLibrary.Devices.Usb;

/// <summary>
/// Represents a service for accessing USB devices.
/// </summary>
public interface IUsbService
{
    /// <summary>
    /// Gets a list of currently connected USB devices.
    /// </summary>
    IEnumerable<IUsbDevice> Devices { get; }

    /// <summary>
    /// Occurs when a device is granted permission to access its data.
    /// </summary>
    event EventHandler<UsbActionEventArgs> DevicePermissionGranted;

    /// <summary>
    /// Occurs when a device is denied permission to access its data.
    /// </summary>
    event EventHandler<UsbActionEventArgs> DevicePermissionDenied;

    /// <summary>
    /// Occurs when a new USB device is attached to the system.
    /// </summary>
    event EventHandler<UsbActionEventArgs> DeviceAttached;

    /// <summary>
    /// Occurs when a USB device is detached from the system.
    /// </summary>
    event EventHandler<UsbActionEventArgs> DeviceDetached;
}