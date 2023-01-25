namespace HermesLibrary.Devices.Ant.Interfaces;

/// <summary>
///     The IAnt interface is used to define the methods for sending and receiving messages using the ANT (Adaptive Network
///     Topology) protocol.
/// </summary>
public interface IAnt
{
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