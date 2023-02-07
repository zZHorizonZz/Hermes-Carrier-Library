namespace HermesCarrierLibrary.Devices.Usb;

/// <summary>
///     The interface representing a USB device.
/// </summary>
public interface IUsbDevice
{
    /// <summary>
    ///     The class code of the device.
    /// </summary>
    int DeviceClass { get; }

    /// <summary>
    ///     The device identifier.
    /// </summary>
    int DeviceId { get; }

    /// <summary>
    ///     The name of the device.
    /// </summary>
    string DeviceName { get; }

    /// <summary>
    ///     The protocol code of the device.
    /// </summary>
    int DeviceProtocol { get; }

    /// <summary>
    ///     The subclass code of the device.
    /// </summary>
    int DeviceSubclass { get; }

    /// <summary>
    ///     The collection of USB interfaces supported by the device.
    /// </summary>
    IEnumerable<IUsbInterface> Interfaces { get; }

    /// <summary>
    ///     The name of the manufacturer of the device.
    /// </summary>
    string ManufacturerName { get; }

    /// <summary>
    ///     The product identifier of the device.
    /// </summary>
    int ProductId { get; }

    /// <summary>
    ///     The name of the product.
    /// </summary>
    string ProductName { get; }

    /// <summary>
    ///     The serial number of the device.
    /// </summary>
    string SerialNumber { get; }

    /// <summary>
    ///     The vendor identifier of the device.
    /// </summary>
    int VendorId { get; }

    /// <summary>
    ///     The version of the device.
    /// </summary>
    string Version { get; }

    /// <summary>
    ///     Indicates whether the application has permission to access the device.
    /// </summary>
    bool HasPermission { get; }

    /// <summary>
    ///     Requests permission to access the device.
    /// </summary>
    void RequestPermission();

    /// <summary>
    ///     Requests permission to access the device from a specific package.
    /// </summary>
    /// <param name="packageName">The package name</param>
    void RequestPermission(string packageName);

    /// <summary>
    ///     Asynchronously requests permission to access the device.
    /// </summary>
    /// <returns>A task representing the asynchronous operation</returns>
    Task RequestPermissionAsync();

    /// <summary>
    ///     Asynchronously requests permission to access the device from a specific package.
    /// </summary>
    /// <param name="packageName">The package name</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task RequestPermissionAsync(string packageName);

    /// <summary>
    ///     Opens the device for communication.
    /// </summary>
    void Open();

    /// <summary>
    ///     Asynchronously opens the device for communication.
    /// </summary>
    /// <returns>A task representing the asynchronous operation</returns>
    Task OpenAsync();

    /// <summary>
    ///     Closes the device for communication.
    /// </summary>
    void Close();

    /// <summary>
    ///     Performs a bulk transfer on the device.
    /// </summary>
    /// <param name="transfer">The bulk transfer object</param>
    /// <returns>The number of bytes transferred</returns>
    int BulkTransfer(UsbBulkTransfer transfer);

    /// <summary>
    ///     Asynchronously performs a bulk transfer on the device.
    /// </summary>
    /// <param name="transfer">The bulk transfer object</param>
    /// <returns>A task representing the asynchronous operation and the number of bytes transferred</returns>
    Task<int> BulkTransferAsync(UsbBulkTransfer transfer);

    /// <summary>
    ///     Performs a control transfer on the device.
    /// </summary>
    /// <param name="transfer">The control transfer object</param>
    /// <returns>The number of bytes transferred</returns>
    int ControlTransfer(UsbControlTransfer transfer);

    /// <summary>
    ///     Asynchronously performs a control transfer on the device.
    /// </summary>
    /// <param name="transfer">The control transfer object</param>
    /// <returns>A task representing the asynchronous operation and the number of bytes transferred</returns>
    Task<int> ControlTransferAsync(UsbControlTransfer transfer);

    /// <summary>
    ///     Claims the specified USB interface for communication.
    /// </summary>
    /// <param name="usbInterface">The USB interface to claim</param>
    void ClaimInterface(IUsbInterface usbInterface);

    /// <summary>
    ///     Asynchronously claims the specified USB interface for communication.
    /// </summary>
    /// <param name="usbInterface">The USB interface to claim</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task ClaimInterfaceAsync(IUsbInterface usbInterface);

    /// <summary>
    ///     Releases the specified USB interface.
    /// </summary>
    /// <param name="usbInterface">The USB interface to release</param>
    void ReleaseInterface(IUsbInterface usbInterface);

    /// <summary>
    ///     Asynchronously releases the specified USB interface.
    /// </summary>
    /// <param name="usbInterface">The USB interface to release</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task ReleaseInterfaceAsync(IUsbInterface usbInterface);

    /// <summary>
    ///     Creates a new USB request object.
    /// </summary>
    /// <returns>The USB request object</returns>
    IUsbRequest CreateRequest();

    /// <summary>
    ///     Asynchronously creates a new USB request object.
    /// </summary>
    /// <returns>A task representing the asynchronous operation and the USB request object</returns>
    Task<IUsbRequest> CreateRequestAsync();
}