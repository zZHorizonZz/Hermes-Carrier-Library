namespace HermesCarrierLibrary.Devices.Usb;

/// <summary>
///     Represents a Universal Serial Bus (USB) request.
/// </summary>
public interface IUsbRequest
{
    /// <summary>
    ///     Gets the endpoint associated with the request.
    /// </summary>
    IUsbEndpoint Endpoint { get; }

    /// <summary>
    ///     Attempts to cancel the request.
    /// </summary>
    /// <returns>
    ///     True if the request was successfully cancelled; otherwise, false.
    /// </returns>
    bool Cancel();

    /// <summary>
    ///     Closes the request.
    /// </summary>
    void Close();

    /// <summary>
    ///     Initializes the request with the specified USB device and endpoint.
    /// </summary>
    /// <param name="device">The USB device associated with the request.</param>
    /// <param name="endpoint">The endpoint associated with the request.</param>
    /// <returns>
    ///     True if the request was successfully initialized; otherwise, false.
    /// </returns>
    bool Initialize(IUsbDevice device, IUsbEndpoint endpoint);

    /// <summary>
    ///     Queues a buffer for the request.
    /// </summary>
    /// <param name="buffer">The buffer to be queued for the request.</param>
    /// <returns>
    ///     True if the buffer was successfully queued for the request; otherwise, false.
    /// </returns>
    bool Queue(byte[] buffer);

    /// <summary>
    ///     Queues a buffer of the specified length for the request.
    /// </summary>
    /// <param name="buffer">The buffer to be queued for the request.</param>
    /// <param name="length">The length of the buffer to be queued for the request.</param>
    /// <returns>
    ///     True if the buffer was successfully queued for the request; otherwise, false.
    /// </returns>
    bool Queue(byte[] buffer, int length);

    /// <summary>
    ///     Waits for the request to complete and returns the result.
    /// </summary>
    /// <param name="device">The USB device associated with the request.</param>
    /// <returns>
    ///     The result of the request.
    /// </returns>
    byte[] RequestWait(IUsbDevice device);

    /// <summary>
    ///     Asynchronously waits for the request to complete and returns the result.
    /// </summary>
    /// <param name="device">The USB device associated with the request.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The result of the task is the result of the request.
    /// </returns>
    Task<byte[]> RequestWaitAsync(IUsbDevice device);

    /// <summary>
    ///     Waits for the request to complete and returns the result, with a specified timeout.
    /// </summary>
    /// <param name="device">The USB device associated with the request.</param>
    /// <param name="timeout">The amount of time to wait for the request to complete.</param>
    /// <returns>
    ///     The result of the request.
    /// </returns>
    byte[] RequestWait(IUsbDevice device, int timeout);

    /// <summary>
    ///     Asynchronously waits for the request to complete and returns the result, with a specified timeout.
    /// </summary>
    /// <param name="device">The USB device associated with the request.</param>
    /// <param name="timeout">The amount of time to wait for the request to complete.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The result of the request.
    /// </returns>
    Task<byte[]> RequestWaitAsync(IUsbDevice device, int timeout);
}