using HermesCarrierLibrary.Devices.Usb.Enum;

namespace HermesCarrierLibrary.Devices.Usb;

/// <summary>
/// Represents a Universal Serial Bus (USB) endpoint.
/// </summary>
public interface IUsbEndpoint
{
    /// <summary>
    /// Gets the address of the endpoint.
    /// </summary>
    int Address { get; }

    /// <summary>
    /// Gets the attributes of the endpoint.
    /// </summary>
    int Attributes { get; }

    /// <summary>
    /// Gets the direction of the endpoint.
    /// </summary>
    UsbDirection Direction { get; }

    /// <summary>
    /// Gets the endpoint number.
    /// </summary>
    int EndpointNumber { get; }

    /// <summary>
    /// Gets the interval, in milliseconds, for polling the endpoint for data transfers.
    /// </summary>
    int Interval { get; }

    /// <summary>
    /// Gets the maximum packet size, in bytes, supported by the endpoint.
    /// </summary>
    int MaxPacketSize { get; }

    /// <summary>
    /// Gets the type of the endpoint.
    /// </summary>
    UsbType Type { get; }
}