using HermesLibrary.Devices.Ant.Channel;

namespace HermesLibrary.Devices.Ant.Interfaces;

/// <summary>
/// The IAntTransmitter interface is used to define the properties and methods of an ANT (Adaptive Network Topology)
/// transmitter device.
/// ANT is a wireless communication protocol used for low-power, short-range wireless communication.
/// </summary>
public interface IAntTransmitter
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
}