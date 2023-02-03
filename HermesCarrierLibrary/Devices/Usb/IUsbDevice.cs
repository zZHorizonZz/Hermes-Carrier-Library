using HermesCarrierLibrary.Devices.Shared;

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

    bool RequestPermission();

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

    void RequestWait();

    Task RequestWaitAsync();

    void RequestWait(int timeout);

    Task RequestWaitAsync(int timeout);

    IUsbRequest CreateRequest();

    Task<IUsbRequest> CreateRequestAsync();
}