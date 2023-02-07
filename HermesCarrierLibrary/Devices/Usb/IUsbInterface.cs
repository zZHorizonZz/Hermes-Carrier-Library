namespace HermesCarrierLibrary.Devices.Usb;

/// <summary>
/// Represents a Universal Serial Bus (USB) interface.
/// </summary>
public interface IUsbInterface
{
    /// <summary>
    /// Gets the endpoints associated with the interface.
    /// </summary>
    IEnumerable<IUsbEndpoint> Endpoints { get; }

    /// <summary>
    /// Gets the identifier of the interface.
    /// </summary>
    int Id { get; }

    /// <summary>
    /// Gets the class code of the interface.
    /// </summary>
    int InterfaceClass { get; }

    /// <summary>
    /// Gets the protocol code of the interface.
    /// </summary>
    int InterfaceProtocol { get; }

    /// <summary>
    /// Gets the subclass code of the interface.
    /// </summary>
    int InterfaceSubclass { get; }

    /// <summary>
    /// Gets the name of the interface.
    /// </summary>
    string Name { get; }
}