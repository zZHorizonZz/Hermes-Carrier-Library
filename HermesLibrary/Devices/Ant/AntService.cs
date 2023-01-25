using HermesLibrary.Devices.Ant.Messages.Client;
using HermesLibrary.Devices.Ant.Messages.Device;
using HermesLibrary.Devices.Shared;

namespace HermesLibrary.Devices.Ant;

public class AntService
{
    public static readonly IAntMessage[] ClientBoundMessages =
    {
        new ResetSystemMessage(),
        new OpenRxScanMode(),
        new AssignChannelMessage(),
        new SetNetworkKeyMessage(),
        new RequestMessage()
    };

    public static readonly IAntMessage[] DeviceBoundMessages =
    {
        new StartUpMessage(),
        new EventResponseMessage(),
        new AntVersionMessage()
    };

    private readonly IUsbService mUsbService;

    public AntService(IUsbService usbService)
    {
        mUsbService = usbService;
    }
}