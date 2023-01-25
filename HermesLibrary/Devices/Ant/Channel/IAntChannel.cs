using HermesLibrary.Devices.Ant.Enum;
using HermesLibrary.Devices.Ant.Interfaces;

namespace HermesLibrary.Devices.Ant.Channel;

/// <summary>
/// The IAntChannel interface is used to define the properties and methods for managing an ANT channel.
/// </summary>
public interface IAntChannel
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

    /// <summary>
    ///     Sends an ANT message asynchronously.
    /// </summary>
    /// <param name="message">The ANT message to send.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SendMessageAsync(IAntMessage message);

    /// <summary>
    ///     Waits for an ANT message to be received and returns the message asynchronously.
    /// </summary>
    /// <param name="message">The ANT message to wait for.</param>
    /// <returns>A task that represents the asynchronous operation, containing the received ANT message.</returns>
    Task<IAntMessage> AwaitMessageAsync(IAntMessage message);

    /// <summary>
    ///     Waits for an ANT message of a specific type to be received and returns the message asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of ANT message to wait for.</typeparam>
    /// <param name="message">The ANT message to wait for.</param>
    /// <returns>A task that represents the asynchronous operation, containing the received ANT message of the specified type.</returns>
    Task<T> AwaitMessageOfTypeAsync<T>(IAntMessage message) where T : IAntMessage;

    /// <summary>
    ///     Receives an ANT message from the data array and returns the message.
    /// </summary>
    /// <param name="data">The data array that contains the ANT message.</param>
    /// <returns>The ANT message received from the data array.</returns>
    IAntMessage ReceiveMessageAsync(byte[] data);
}