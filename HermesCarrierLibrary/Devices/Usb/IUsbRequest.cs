namespace HermesCarrierLibrary.Devices.Usb;

public interface IUsbRequest
{
    IUsbEndpoint Endpoint { get; }

    bool Cancel();

    void Close();

    bool Initialize(IUsbDevice device, IUsbEndpoint endpoint);

    bool Queue(byte[] buffer);

    bool Queue(byte[] buffer, int length);
}