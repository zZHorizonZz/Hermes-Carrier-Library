using HermesLibrary.Devices.Ant.Interfaces;

namespace HermesLibrary.Devices.Ant.EventArgs;

public class AntTransmitterStatusChangedEventArgs : System.EventArgs
{
    public AntTransmitterStatusChangedEventArgs(IAntTransmitter transmitter)
    {
        Transmitter = transmitter;
    }

    public IAntTransmitter Transmitter { get; }
}