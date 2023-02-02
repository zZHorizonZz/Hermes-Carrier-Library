using HermesCarrierLibrary.Devices.Ant.Channel;
using HermesCarrierLibrary.Devices.Ant.Enum;
using HermesCarrierLibrary.Devices.Ant.Messages;

namespace HermesCarrierLibrary.Devices.Ant.Interfaces;

/// <summary>
///     The IAntTransmitter interface is used to define the properties and methods of an ANT (Adaptive Network Topology)
///     transmitter device.
///     ANT is a wireless communication protocol used for low-power, short-range wireless communication.
/// </summary>
public interface IAntTransmitter : IAntMessenger
{
    string AntVersion { get; }

    string SerialNumber { get; }

    IEnumerable<Capabilities> Capabilities { get; }

    /// <summary>
    ///     Gets a value indicating whether the serial connection is currently open.
    /// </summary>
    bool IsConnected { get; }

    /// <summary>
    ///     Gets the Dictionary of currently active channels for the ANT transmitter.
    /// </summary>
    IDictionary<byte, IAntChannel> ActiveChannels { get; }

    /// <summary>
    ///     Set network key for the ANT transmitter.
    /// </summary>
    /// <param name="networkNumber">The network number to set the key for.</param>
    /// <param name="key">The network key to set.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SetNetworkKeyAsync(byte networkNumber, byte[] key);

    /// <summary>
    ///     Open the new ANT channel.
    /// </summary>
    /// <param name="channel">The channel to open.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task OpenChannelAsync(IAntChannel channel);

    /// <summary>
    ///     Close the ANT channel.
    /// </summary>
    /// <param name="channel">The channel to close.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task CloseChannelAsync(IAntChannel channel);

    /// <summary>
    ///     Checks if the channel is open.
    /// </summary>
    /// <param name="channelNumber">The channel number to check.</param>
    /// <returns>True if the channel is open, false otherwise.</returns>
    bool IsChannelOpen(byte channelNumber);
}