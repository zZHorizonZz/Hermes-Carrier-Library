namespace HermesCarrierLibrary.Devices.Usb;

public interface IUsbRequest
{
    IUsbEndpoint Endpoint { get; }

    bool Cancel();

    void Close();

    bool Initialize(IUsbDevice device, IUsbEndpoint endpoint);

    bool Queue(byte[] buffer);

    bool Queue(byte[] buffer, int length);

    byte[] RequestWait(IUsbDevice device);

    Task<byte[]> RequestWaitAsync(IUsbDevice device);

    byte[] RequestWait(IUsbDevice device, int timeout);

    Task<byte[]> RequestWaitAsync(IUsbDevice device, int timeout);
}