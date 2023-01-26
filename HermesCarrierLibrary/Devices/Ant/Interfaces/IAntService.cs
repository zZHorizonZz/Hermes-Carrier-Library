using HermesCarrierLibrary.Devices.Ant.EventArgs;

namespace HermesCarrierLibrary.Devices.Ant.Interfaces;

public interface IAntService
{
    IAntTransmitter? CurrentTransmitter { get; }

    event EventHandler<AntTransmitterStatusChangedEventArgs> TransmitterStatusChanged;

    IAntTransmitter[] DetectTransmitters();

    void ConnectTransmitter(IAntTransmitter transmitter);

    void DisconnectTransmitter(IAntTransmitter transmitter);
}