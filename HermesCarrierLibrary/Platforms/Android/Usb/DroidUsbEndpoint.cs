using Android.Hardware.Usb;
using HermesCarrierLibrary.Devices.Usb;
using HermesCarrierLibrary.Devices.Usb.Enum;

namespace HermesCarrierLibrary.Platforms.Android.Devices;

public class DroidUsbEndpoint : IUsbEndpoint
{
    internal readonly UsbEndpoint Endpoint;

    public DroidUsbEndpoint(UsbEndpoint usbEndpoint)
    {
        Endpoint = usbEndpoint;
    }

    /// <inheritdoc />
    public int Address => (int)Endpoint.Address;

    /// <inheritdoc />
    public int Attributes => Endpoint.Attributes;

    /// <inheritdoc />
    public UsbDirection Direction => (UsbDirection)Endpoint.Direction;

    /// <inheritdoc />
    public int EndpointNumber => Endpoint.EndpointNumber;

    /// <inheritdoc />
    public int Interval => Endpoint.Interval;

    /// <inheritdoc />
    public int MaxPacketSize => Endpoint.MaxPacketSize;

    /// <inheritdoc />
    public UsbType Type => (UsbType)Endpoint.Type;
}