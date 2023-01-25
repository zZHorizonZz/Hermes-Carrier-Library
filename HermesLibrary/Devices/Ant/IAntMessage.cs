namespace HermesLibrary.Devices.Ant;

/// <summary>
/// The IAntMessage interface is used to define the properties and methods for encoding and decoding ANT messages.
/// </summary>
public interface IAntMessage
{
    /// <summary>
    /// Gets or sets the length of the ANT message.
    /// </summary>
    byte Length { get; set; }

    /// <summary>
    /// Gets or sets the message ID of the ANT message.
    /// </summary>
    byte MessageId { get; set; }

    /// <summary>
    /// Decodes the ANT message from a data array.
    /// </summary>
    /// <param name="data">The data array that contains the ANT message.</param>
    void Decode(byte[] data);

    /// <summary>
    /// Decodes the payload of the ANT message from a BinaryReader object.
    /// </summary>
    /// <param name="reader">The BinaryReader object that contains the payload of the ANT message.</param>
    void DecodePayload(BinaryReader reader);

    /// <summary>
    /// Encodes the ANT message into a data array.
    /// </summary>
    /// <returns>The data array that contains the encoded ANT message.</returns>
    byte[] Encode();

    /// <summary>
    /// Encodes the payload of the ANT message into a BinaryWriter object.
    /// </summary>
    /// <returns>The BinaryWriter object that contains the encoded payload of the ANT message.</returns>
    BinaryWriter EncodePayload();
}