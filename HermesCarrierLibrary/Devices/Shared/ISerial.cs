namespace HermesCarrierLibrary.Devices.Shared;

/// <summary>
///     The ISerial interface is used to define the methods for reading and writing data to a serial communication device.
/// </summary>
public interface ISerial
{
    /// <summary>
    ///     Gets a value indicating whether the serial connection is currently open.
    /// </summary>
    bool IsConnected { get; }

    /// <summary>
    ///     The name of the device.
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     Vendor ID of the device.
    /// </summary>
    int VendorId { get; }

    /// <summary>
    ///     Device ID of the device.
    /// </summary>
    int ProductId { get; }

    /// <summary>
    ///     Occurs when the serial connection is opened.
    /// </summary>
    event EventHandler Opened;

    /// <summary>
    ///     Occurs when the serial connection is closed.
    /// </summary>
    event EventHandler Closed;

    /// <summary>
    ///     Writes data to the serial connection.
    /// </summary>
    /// <param name="data">The data to be written to the serial connection.</param>
    void Write(byte[] data);

    /// <summary>
    ///     Reads data from the serial connection.
    /// </summary>
    /// <returns>The data read from the serial connection.</returns>
    byte[] Read();

    /// <summary>
    ///     Reads a specified number of bytes from the serial connection.
    /// </summary>
    /// <param name="length">The number of bytes to read from the serial connection.</param>
    /// <returns>The data read from the serial connection.</returns>
    byte[] Read(int length);
}