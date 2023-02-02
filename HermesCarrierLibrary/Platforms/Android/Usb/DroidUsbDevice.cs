using Android.App;
using Android.Content;
using Android.Hardware.Usb;
using HermesCarrierLibrary.Devices.Usb;
using Microsoft.Extensions.Logging;

namespace HermesCarrierLibrary.Platforms.Android.Devices;

public class DroidUsbDevice : IUsbDevice
{
    public const string ActionUsbPermission = "com.hermes.carrier.USB_PERMISSION";

    internal readonly global::Android.Hardware.Usb.UsbDevice Device;

    private readonly Context mContext;

    private readonly ILogger<DroidUsbDevice> mLogger = new LoggerFactory().CreateLogger<DroidUsbDevice>();
    private readonly UsbManager mUsbManager;
    internal UsbDeviceConnection DeviceConnection;

    public DroidUsbDevice(global::Android.Hardware.Usb.UsbDevice device, Context context,
        UsbManager usbManager)
    {
        Device = device;
        mContext = context;
        mUsbManager = usbManager;
        Interfaces = GetInterfaces(device);
    }

    /// <inheritdoc />
    public int DeviceClass => (int)Device.DeviceClass;

    /// <inheritdoc />
    public int DeviceId => Device.DeviceId;

    /// <inheritdoc />
    public string DeviceName => Device.DeviceName;

    /// <inheritdoc />
    public int DeviceProtocol => Device.DeviceProtocol;

    /// <inheritdoc />
    public int DeviceSubclass => (int)Device.DeviceSubclass;

    /// <inheritdoc />
    public IEnumerable<IUsbInterface> Interfaces { get; }

    /// <inheritdoc />
    public string ManufacturerName => Device.ManufacturerName;

    /// <inheritdoc />
    public int ProductId => Device.ProductId;

    /// <inheritdoc />
    public string ProductName => Device.ProductName;

    /// <inheritdoc />
    public string SerialNumber => Device.SerialNumber;

    /// <inheritdoc />
    public int VendorId => Device.VendorId;

    /// <inheritdoc />
    public string Version => Device.Version;

    /// <inheritdoc />
    public bool HasPermission => CheckPermission();

    /// <inheritdoc />
    public bool RequestPermission()
    {
        return RequestPermission(ActionUsbPermission);
    }

    /// <inheritdoc />
    public void Open()
    {
        if (!HasPermission) throw new Exception("Device does not have permission");

        DeviceConnection = mUsbManager.OpenDevice(Device);
    }

    /// <inheritdoc />
    public Task OpenAsync()
    {
        return Task.Run(Open);
    }

    /// <inheritdoc />
    public void Close()
    {
        DeviceConnection?.Close();
    }

    /// <inheritdoc />
    public Task CloseAsync()
    {
        return Task.Run(Close);
    }

    /// <inheritdoc />
    public int BulkTransfer(UsbBulkTransfer transfer)
    {
        var endpoint = transfer.Endpoint;
        if (endpoint is not DroidUsbEndpoint usbEndpoint) throw new Exception("Endpoint is not a valid USB endpoint");

        return DeviceConnection.BulkTransfer(usbEndpoint.Endpoint, transfer.Buffer, transfer.Length,
            transfer.Timeout);
    }

    /// <inheritdoc />
    public async Task<int> BulkTransferAsync(UsbBulkTransfer transfer)
    {
        var endpoint = transfer.Endpoint;
        if (endpoint is not DroidUsbEndpoint usbEndpoint) throw new Exception("Endpoint is not a valid USB endpoint");

        return await DeviceConnection.BulkTransferAsync(usbEndpoint.Endpoint, transfer.Buffer, transfer.Length,
            transfer.Timeout);
    }

    /// <inheritdoc />
    public int ControlTransfer(UsbControlTransfer transfer)
    {
        var type = transfer.RequestType;
        UsbAddressing? droidAddressing = null;

        foreach (var value in Enum.GetValues<UsbAddressing>())
        {
            if ((int)value != (int)type) continue;

            droidAddressing = value;
            break;
        }

        if (!droidAddressing.HasValue) throw new Exception("Invalid request type");

        return DeviceConnection.ControlTransfer(droidAddressing.Value, transfer.Request, transfer.Value,
            transfer.Index, transfer.Data, transfer.Length, transfer.Timeout);
    }

    /// <inheritdoc />
    public async Task<int> ControlTransferAsync(UsbControlTransfer transfer)
    {
        var type = transfer.RequestType;
        UsbAddressing? droidAddressing = null;

        foreach (var value in Enum.GetValues<UsbAddressing>())
        {
            if ((int)value != (int)type) continue;

            droidAddressing = value;
            break;
        }

        if (!droidAddressing.HasValue) throw new Exception("Invalid request type");

        return await DeviceConnection.ControlTransferAsync(droidAddressing.Value, transfer.Request, transfer.Value,
            transfer.Index, transfer.Data, transfer.Length, transfer.Timeout);
    }

    /// <inheritdoc />
    public void ClaimInterface(IUsbInterface usbInterface)
    {
        if (DeviceConnection is null) throw new Exception("Device is not open");

        if (usbInterface is not DroidUsbInterface droidInterface)
            throw new Exception("Interface is not a valid USB interface");

        DeviceConnection.ClaimInterface(droidInterface.Interface, true);
    }

    /// <inheritdoc />
    public Task ClaimInterfaceAsync(IUsbInterface usbInterface)
    {
        return Task.Run(() => ClaimInterface(usbInterface));
    }

    /// <inheritdoc />
    public void ReleaseInterface(IUsbInterface usbInterface)
    {
        if (DeviceConnection is null) throw new Exception("Device is not open");

        if (usbInterface is not DroidUsbInterface droidInterface)
            throw new Exception("Interface is not a valid USB interface");

        DeviceConnection.ReleaseInterface(droidInterface.Interface);
    }

    /// <inheritdoc />
    public Task ReleaseInterfaceAsync(IUsbInterface usbInterface)
    {
        return Task.Run(() => ReleaseInterface(usbInterface));
    }

    /// <inheritdoc />
    public void RequestWait()
    {
        DeviceConnection.RequestWait().
    }

    /// <inheritdoc />
    public Task RequestWaitAsync()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public void RequestWait(int timeout)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task RequestWaitAsync(int timeout)
    {
        throw new NotImplementedException();
    }

    private static IEnumerable<IUsbInterface> GetInterfaces(global::Android.Hardware.Usb.UsbDevice device)
    {
        var interfaces = new List<IUsbInterface>();
        for (var i = 0; i < device.InterfaceCount; i++) interfaces.Add(new DroidUsbInterface(device.GetInterface(i)));

        return interfaces;
    }

    private bool CheckPermission()
    {
        return mUsbManager.HasPermission(Device);
    }

    public bool RequestPermission(string packageName)
    {
        try
        {
            var pendingIntent = PendingIntent.GetBroadcast(mContext,
                0,
                new Intent(ActionUsbPermission),
                PendingIntentFlags.Mutable);
            mUsbManager.RequestPermission(Device, pendingIntent);

            mLogger.LogInformation("Requesting permission for device {0}", Device.DeviceName);
            return true;
        }
        catch (Exception e)
        {
            mLogger.LogError(e, "Error requesting permission for device {0}", Device.DeviceName);
            return false;
        }
    }

    public override bool Equals(object? obj)
    {
        if (obj is DroidUsbDevice usbDevice) return usbDevice.DeviceId == DeviceId;

        return false;
    }

    public override string ToString()
    {
        return $"Device: {DeviceName} - {ManufacturerName} {ProductName} {SerialNumber} - {VendorId}:{ProductId}";
    }
}