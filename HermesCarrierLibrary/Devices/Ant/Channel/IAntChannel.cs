using HermesCarrierLibrary.Devices.Ant.Enum;
using HermesCarrierLibrary.Devices.Ant.Interfaces;
using HermesCarrierLibrary.Devices.Ant.Messages;

namespace HermesCarrierLibrary.Devices.Ant.Channel;

/// <summary>
///     The IAntChannel interface is used to define the properties and methods for managing an ANT channel.
/// </summary>
public interface IAntChannel : IAntMessenger
{
    /// <summary>
    ///     Gets the channel number of the ANT channel.
    /// </summary>
    byte Number { get; }

    /// <summary>
    ///     Gets the network number of the ANT channel.
    /// </summary>
    byte NetworkNumber { get; }

    /// <summary>
    ///     Gets the type of the ANT channel.
    /// </summary>
    ChannelType Type { get; }

    /// <summary>
    ///     Gets the extended assignment type of the ANT channel.
    /// </summary>
    ExtendedAssignmentType ExtendedAssignment { get; }

    /// <summary>
    ///     Gets the period of the ANT channel.
    /// </summary>
    ushort Period { get; }

    /// <summary>
    ///     Gets the frequency of the ANT channel.
    /// </summary>
    byte Frequency { get; }

    Task AssignChannel(IAntTransmitter transmitter);

    Task Open();

    /// <summary>
    ///     Closes the ANT channel.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task Close();
}