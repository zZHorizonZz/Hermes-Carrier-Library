using HermesCarrierLibrary.Devices.Ant.Interfaces;

namespace HermesCarrierLibrary.Devices.Ant.EventArgs;

public class AntTransmitterStatusChangedEventArgs : System.EventArgs
{
    public AntTransmitterStatusChangedEventArgs(IAntTransmitter transmitter, Status status)
    {
        Transmitter = transmitter;
        Status = status;
    }

    public IAntTransmitter Transmitter { get; }

    public Status Status { get; set; }
}

public enum Status
{
    Connected,
    Disconnected
}