namespace HermesCarrierLibrary.Devices.Ant.Interfaces;

/// <summary>
///     The interface representing the AntService
/// </summary>
public interface IAntService
{
    /// <summary>
    ///     The dictionary containing all the connected AntTransmitters,
    ///     where the key is the identifier of the transmitter.
    /// </summary>
    IDictionary<int, IAntTransmitter> Transmitters { get; }

    /// <summary>
    ///     Detects available AntTransmitters it can be connected to.
    /// For example if there is a ANT+ USB stick connected to the device it will be detected.
    /// Or if the device has a built-in ANT+ receiver it will be detected.
    /// </summary>
    /// <returns>An array of AntTransmitters</returns>
    IAntTransmitter[] DetectTransmitters();

    /// <summary>
    ///     Connects to the specified AntTransmitter.
    /// </summary>
    /// <param name="transmitter">The AntTransmitter to connect to</param>
    void ConnectTransmitter(IAntTransmitter transmitter);

    /// <summary>
    ///     Disconnects from the specified AntTransmitter.
    /// </summary>
    /// <param name="transmitter">The AntTransmitter to disconnect from</param>
    void DisconnectTransmitter(IAntTransmitter transmitter);
}