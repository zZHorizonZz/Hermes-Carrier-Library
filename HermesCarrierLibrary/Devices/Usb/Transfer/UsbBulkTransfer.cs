namespace HermesCarrierLibrary.Devices.Usb;

public class UsbBulkTransfer
{
    public UsbBulkTransfer(IUsbEndpoint endpoint, byte[] buffer, int offset, int length, int timeout)
    {
        Endpoint = endpoint;
        Buffer = buffer;
        Offset = offset;
        Length = length;
        Timeout = timeout;
    }

    public UsbBulkTransfer(IUsbEndpoint endpoint, byte[] buffer, int length, int timeout)
    {
        Endpoint = endpoint;
        Buffer = buffer;
        Length = length;
        Timeout = timeout;
    }

    public IUsbEndpoint Endpoint { get; }

    public byte[] Buffer { get; }

    public int? Offset { get; }

    public int Length { get; }

    public int Timeout { get; }
}