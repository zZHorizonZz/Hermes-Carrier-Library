using HermesCarrierLibrary.Devices.Ant;
using HermesCarrierLibrary.Devices.Ant.Channel;
using HermesCarrierLibrary.Devices.Ant.Dongle;
using HermesCarrierLibrary.Devices.Ant.Enum;
using HermesCarrierLibrary.Devices.Ant.EventArgs;
using HermesCarrierLibrary.Devices.Ant.Interfaces;
using HermesCarrierLibrary.Devices.Ant.Messages.Client;
using HermesCarrierLibrary.Devices.Ant.Util;
using HermesCarrierLibrary.Devices.Shared;
#if ANDROID
using HermesCarrierLibrary.Platforms.Android.Devices;
#endif

namespace HermesCarrierLibrary.Devices;

public class DeviceService
{
    public DeviceService()
    {
#if ANDROID
        UsbService = AndroidDeviceService.Current?.UsbService;
        UsbService.DevicePermissionGranted += OnConnectSerial;
        UsbService.DeviceDetached += OnDisconnectSerial;
#endif

        if (UsbService is null)
            throw new NullReferenceException("UsbService is null");

        AntService = new AntService(UsbService);
        AntService.TransmitterStatusChanged += OnTransmitterStatusChanged;
    }

    public IUsbService? UsbService { get; init; }
    public IAntService AntService { get; init; }

    public void OnConnectSerial(object? sender, UsbActionEventArgs args)
    {
        Console.WriteLine("OnConnectSerial");
        if (args.Device.IsAntDongle())
        {
            Console.WriteLine("OnConnectSerial - IsAntDongle");
            var transmitter = new AntDongleTransmitter(args.Device);
            AntService.ConnectTransmitter(transmitter);
            return;
        }

        Console.WriteLine("OnConnectSerial - End");
    }

    public void OnDisconnectSerial(object? sender, UsbActionEventArgs args)
    {
        if (args.Device.IsAntDongle())
        {
        }
    }

    public void OnTransmitterStatusChanged(object? sender, AntTransmitterStatusChangedEventArgs args)
    {
        var transmitter = args.Transmitter;
        Console.WriteLine($"OnTransmitterStatusChanged: {transmitter.IsConnected}");

        Task.Run(async () =>
        {
            await transmitter.SendMessageAsync(new ResetSystemMessage());
            Thread.Sleep(500);
            await transmitter.SendMessageAsync(new SetNetworkKeyMessage(1,
                new byte[] { 0xF9, 0xED, 0x22, 0xB8, 0xFD, 0x56, 0x67, 0xCD }));
            var channel = new Channel(0, 1, ChannelType.TransmitChannel, ExtendedAssignmentType.UNKNOWN, 0x2000,
                0x03);

            await transmitter.OpenChannelAsync(channel);

            Thread.Sleep(500);
            var lastAcknowledgeTime = DateTime.Now;
            while (transmitter.IsConnected)
            {
                if (DateTime.Now > lastAcknowledgeTime.AddSeconds(1))
                {
                    Console.WriteLine("Sending Acknowledge");
                    await channel.SendMessageAsync(new AcknowledgedDataMessage(new byte[]
                        { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }));
                    lastAcknowledgeTime = DateTime.Now;
                }
                else
                {
                    await channel.SendMessageAsync(new BroadcastDataMessage(new byte[]
                        { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff }));
                }

                Thread.Sleep(250);
            }
        });
    }
}