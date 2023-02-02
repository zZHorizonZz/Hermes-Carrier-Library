using HermesCarrierLibrary.Devices.Usb;

namespace HermesCarrierLibrary.Platforms.Android.Devices;

public class DroidUsbInterface : IUsbInterface
{
    internal readonly global::Android.Hardware.Usb.UsbInterface Interface;

    public DroidUsbInterface(global::Android.Hardware.Usb.UsbInterface usbInterface)
    {
        Interface = usbInterface;
        Endpoints = GetEndpoints(usbInterface);
    }

    /// <inheritdoc />
    public IEnumerable<IUsbEndpoint> Endpoints { get; }

    /// <inheritdoc />
    public int Id => Interface.Id;

    /// <inheritdoc />
    public int InterfaceClass => (int)Interface.InterfaceClass;

    /// <inheritdoc />
    public int InterfaceProtocol => Interface.InterfaceProtocol;

    /// <inheritdoc />
    public int InterfaceSubclass => (int)Interface.InterfaceSubclass;

    /// <inheritdoc />
    public string Name => Interface.Name;

    private static IEnumerable<IUsbEndpoint> GetEndpoints(global::Android.Hardware.Usb.UsbInterface usbInterface)
    {
        var endpoints = new List<IUsbEndpoint>();
        for (var i = 0; i < usbInterface.EndpointCount; i++)
            endpoints.Add(new DroidUsbEndpoint(usbInterface.GetEndpoint(i)));

        return endpoints;
    }
}