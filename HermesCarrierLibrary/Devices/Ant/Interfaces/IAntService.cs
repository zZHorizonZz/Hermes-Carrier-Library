namespace HermesCarrierLibrary.Devices.Ant.Interfaces;

public interface IAntService
{
    IDictionary<int, IAntTransmitter> Transmitters { get; }

    IAntTransmitter[] DetectTransmitters();

    void ConnectTransmitter(IAntTransmitter transmitter);

    void DisconnectTransmitter(IAntTransmitter transmitter);
}