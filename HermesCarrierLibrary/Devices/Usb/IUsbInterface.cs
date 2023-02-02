namespace HermesCarrierLibrary.Devices.Usb;

public interface IUsbInterface
{
    IEnumerable<IUsbEndpoint> Endpoints { get; }

    int Id { get; }

    int InterfaceClass { get; }

    int InterfaceProtocol { get; }

    int InterfaceSubclass { get; }

    string Name { get; }
}