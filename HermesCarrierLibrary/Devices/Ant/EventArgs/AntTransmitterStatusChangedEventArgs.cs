using HermesCarrierLibrary.Devices.Ant.Interfaces;

namespace HermesCarrierLibrary.Devices.Ant.EventArgs;

public class AntTransmitterStatusChangedEventArgs : System.EventArgs
{
    public AntTransmitterStatusChangedEventArgs(IAntTransmitter transmitter)
    {
        Transmitter = transmitter;
    }

    public IAntTransmitter Transmitter { get; }
}