using HermesCarrierLibrary.Devices.Ant.Enum;
using HermesCarrierLibrary.Devices.Ant.Interfaces;
using HermesCarrierLibrary.Devices.Ant.Messages;

namespace HermesCarrierLibrary.Devices.Ant.Channel;

/// <summary>
/// The IAntChannel interface is used to define the properties and methods for managing an ANT channel.
/// </summary>
public interface IAntChannel : IAntMessenger
{
    /// <summary>
    /// Gets the channel number of the ANT channel.
    /// </summary>
    byte Number { get; }

    /// <summary>
    /// Gets the network number of the ANT channel.
    /// </summary>
    byte NetworkNumber { get; }

    /// <summary>
    /// Gets the type of the ANT channel.
    /// </summary>
    ChannelType Type { get; }

    /// <summary>
    /// Gets the extended assignment type of the ANT channel.
    /// </summary>
    ExtendedAssignmentType ExtendedAssignment { get; }

    /// <summary>
    /// Gets the period of the ANT channel.
    /// </summary>
    ushort Period { get; }

    /// <summary>
    /// Gets the frequency of the ANT channel.
    /// </summary>
    byte Frequency { get; }

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
    /// Opens the ANT channel and associates it with an ANT transmitter.
    /// </summary>
    /// <param name="transmitter">The ANT transmitter to associate the channel with.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task Open(IAntTransmitter transmitter);

    /// <summary>
    /// Closes the ANT channel.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task Close();
}