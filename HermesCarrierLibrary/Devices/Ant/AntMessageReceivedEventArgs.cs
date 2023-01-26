namespace HermesCarrierLibrary.Devices.Ant;

public class AntMessageReceivedEventArgs : System.EventArgs
{
    public AntMessageReceivedEventArgs(IAntMessage message)
    {
        Message = message;
    }

    public IAntMessage Message { get; }
}