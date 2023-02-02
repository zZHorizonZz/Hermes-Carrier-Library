using HermesCarrierLibrary.Devices.Usb.Enum;

namespace HermesCarrierLibrary.Devices.Usb;

public interface IUsbEndpoint
{
    int Address { get; }

    int Attributes { get; }

    UsbDirection Direction { get; }

    int EndpointNumber { get; }

    int Interval { get; }

    int MaxPacketSize { get; }

    UsbType Type { get; }
}