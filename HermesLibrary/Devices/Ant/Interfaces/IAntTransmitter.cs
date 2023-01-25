using HermesLibrary.Devices.Ant.Channel;
using HermesLibrary.Devices.Ant.Messages;

namespace HermesLibrary.Devices.Ant.Interfaces;

/// <summary>
/// The IAntTransmitter interface is used to define the properties and methods of an ANT (Adaptive Network Topology)
/// transmitter device.
/// ANT is a wireless communication protocol used for low-power, short-range wireless communication.
/// </summary>
public interface IAntTransmitter : IAntMessenger
{
    /// <summary>
    /// Gets the unique device number assigned to the ANT transmitter.
    /// </summary>
    ushort DeviceNumber { get; }

    /// <summary>
    /// Gets a value indicating whether the ANT transmitter is configured as a slave or a master device.
    /// </summary>
    bool IsSlave { get; }

    /// <summary>
    /// Gets the type of device the ANT transmitter is communicating with.
    /// </summary>
    byte DeviceType { get; }

    /// <summary>
    /// Gets the type of transmission being used by the ANT transmitter, such as asynchronous or synchronous.
    /// </summary>
    byte TransmissionType { get; }

    /// <summary>
    /// Gets the Dictionary of currently active channels for the ANT transmitter.
    /// </summary>
    Dictionary<byte, IAntChannel> ActiveChannels { get; }

    /// <summary>
    /// Set network key for the ANT transmitter.
    /// </summary>
    /// <param name="networkNumber">The network number to set the key for.</param>
    /// <param name="key">The network key to set.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SetNetworkKeyAsync(byte networkNumber, byte[] key);

    //TODO: Remove advanced message and add channel number as default argument to basic message
}