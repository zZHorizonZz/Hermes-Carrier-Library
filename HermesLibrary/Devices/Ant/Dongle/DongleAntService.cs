using HermesLibrary.Devices.Ant.Channel;
using HermesLibrary.Devices.Ant.Interfaces;
using HermesLibrary.Devices.Ant.Messages.Client;
using HermesLibrary.Devices.Ant.Messages.Device;
using QuickStat.Platforms.Android.Devices.Ant.Dongle;
using Application = Android.App.Application;

namespace HermesLibrary.Devices.Ant.Dongle;

public class DongleAntService
{
    public DongleAntService()
    {
    }

    public DongleAntService(IAntTransmitter transmitter)
    {
        if (DongleTransmitter is not AntDongleTransmitter)
            throw new ArgumentException("Transmitter must be an AntDongleTransmitter");

        DongleTransmitter = transmitter;
    }

    public IAntTransmitter DongleTransmitter { get; }

    public void OnDevicePermissionGranted(UsbDevice device)
    {
        if (IsAntDongle(device))
        {
            var serial = new UsbSerial(device);
            var dongle = new AntDongleTransmitter(serial);

            mDongleTransmitters.Add(dongle);

            serial.Open(Application.Context);

            var toast = new Toast { Text = "Dongle connected " + device.ProductName };
            toast.Show();

            Task.Run(async () =>
            {
                await dongle.SendMessageAsync(new ResetSystemMessage());
                Thread.Sleep(500);
                await dongle.SendMessageAsync(new SetNetworkKeyMessage(1,
                    new byte[] { 0xF9, 0xED, 0x22, 0xB8, 0xFD, 0x56, 0x67, 0xCD }));
                var channel = new Channel.Channel(0, 1, ChannelType.TransmitChannel)
                {
                    Period = 0x2000,
                    Frequency = 0x03
                };

                await channel.Open(dongle, dongle);

                Thread.Sleep(500);
                var lastAcknowledgeTime = DateTime.Now;
                while (serial.IsConnected)
                {
                    if (DateTime.Now > lastAcknowledgeTime.AddSeconds(1))
                    {
                        await channel.SendMessage(new AcknowledgedDataMessage(0,
                            new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }));
                        lastAcknowledgeTime = DateTime.Now;
                    }
                    else
                    {
                        await channel.SendMessage(new BroadcastDataMessage(0,
                            new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff }));
                    }

                    Thread.Sleep(250);
                }
            });
        }
        else
        {
            var toast = new Toast { Text = "Not an ANT+ dongle " + device.ProductName };
            toast.Show();
        }
    }

    private void OnMessageReceived(object? sender, AntMessageReceivedEventArgs args)
    {
        switch (args.Message)
        {
            case StartUpMessage startUpMessage:
                Console.WriteLine(startUpMessage);
                break;
            case EventResponseMessage eventResponseMessage:
                Console.WriteLine(eventResponseMessage);
                break;
            case AntVersionMessage antVersionMessage:
                Console.WriteLine(antVersionMessage);
                break;
            case UnknownMessage unknownMessage:
                Console.WriteLine("Unknown message: " + BitConverter.ToString(unknownMessage.Data));
                break;
        }
    }

    public void OnDevicePermissionDenied(UsbDevice device)
    {
        var toast = new Toast { Text = "Permission denied for " + device.ProductName };
        toast.Show();
    }

    public void OnDeviceDetached(UsbDevice device)
    {
        var dongle = mDongleTransmitters.FirstOrDefault(d => d.DeviceId == device.DeviceId);
        if (dongle != null)
        {
            mDongleTransmitters.Remove(dongle);

            var toast = new Toast { Text = "Dongle disconnected " + device.ProductName };
            toast.Show();
        }
    }

    public bool IsAntDongle(UsbDevice? device)
    {
        if (device == null)
            return false;

        if (device.VendorId != 0x0fcf) return false;

        return device.ProductId is 0x1008 or 0x1009;
    }
}