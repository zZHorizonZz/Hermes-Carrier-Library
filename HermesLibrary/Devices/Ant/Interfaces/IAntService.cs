using HermesLibrary.Devices.Ant.EventArgs;

namespace HermesLibrary.Devices.Ant.Interfaces;

public interface IAntService
{
    IAntMessage[] ClientBoundMessages { get; }

    IAntMessage[] DeviceBoundMessages { get; }

    IAntTransmitter? CurrentTransmitter { get; }

    event EventHandler<AntTransmitterStatusChangedEventArgs> TransmitterStatusChanged;

    IAntTransmitter[] DetectTransmitters();

    void ConnectTransmitter(IAntTransmitter transmitter);

    void DisconnectTransmitter(IAntTransmitter transmitter);
}