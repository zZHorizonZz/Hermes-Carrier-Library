namespace HermesCarrierLibrary.Devices.Usb;

public interface IUsbDevice
{
    int DeviceClass { get; }

    int DeviceId { get; }

    string DeviceName { get; }

    int DeviceProtocol { get; }

    int DeviceSubclass { get; }

    IEnumerable<IUsbInterface> Interfaces { get; }

    string ManufacturerName { get; }

    int ProductId { get; }

    string ProductName { get; }

    string SerialNumber { get; }

    int VendorId { get; }

    string Version { get; }

    bool HasPermission { get; }

    void RequestPermission();

    void RequestPermission(string packageName);

    Task RequestPermissionAsync();

    Task RequestPermissionAsync(string packageName);

    void Open();

    Task OpenAsync();

    void Close();

    Task CloseAsync();

    int BulkTransfer(UsbBulkTransfer transfer);

    Task<int> BulkTransferAsync(UsbBulkTransfer transfer);

    int ControlTransfer(UsbControlTransfer transfer);

    Task<int> ControlTransferAsync(UsbControlTransfer transfer);

    void ClaimInterface(IUsbInterface usbInterface);

    Task ClaimInterfaceAsync(IUsbInterface usbInterface);

    void ReleaseInterface(IUsbInterface usbInterface);

    Task ReleaseInterfaceAsync(IUsbInterface usbInterface);

    IUsbRequest CreateRequest();

    Task<IUsbRequest> CreateRequestAsync();
}