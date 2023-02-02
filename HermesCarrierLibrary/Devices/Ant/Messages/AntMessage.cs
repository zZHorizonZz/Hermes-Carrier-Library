using HermesCarrierLibrary.Devices.Ant.Interfaces;
using HermesCarrierLibrary.Devices.Ant.Util;

namespace HermesCarrierLibrary.Devices.Ant.Messages;

public abstract class AntMessage : IAntMessage
{
    public AntMessage(byte messageId, byte length)
    {
        MessageId = messageId;
        Length = length;
    }

    public byte MessageId { get; set; }
    public byte Length { get; set; }

    public byte ChannelNumber { get; set; }

    /// <inheritdoc />
    public abstract void DecodePayload(BinaryReader payload);

    /// <inheritdoc />
    public void Decode(byte[] data)
    {
        var sync = data[0];
        var length = data[1];
        var messageId = data[2];

        if (sync != 0xA4) throw new ArgumentException("Invalid sync byte");

        var payload = new byte[length];
        for (var i = 0; i < length; i++) payload[i] = data[i + 3];

        Length = length;
        MessageId = messageId;

        var checksum = data[^1];
        var calculatedChecksum = data.CalculateChecksum();

        if (checksum != calculatedChecksum) throw new ArgumentException("Invalid checksum");

        var payloadStream = new MemoryStream(payload);
        var reader = new BinaryReader(payloadStream);

        DecodePayload(reader);

        reader.Close();
        payloadStream.Close();
    }

    /// <inheritdoc />
    public byte[] Encode()
    {
        var writer = EncodePayload();
        var payload = new byte[writer.BaseStream.Length];

        var length = (byte)payload.Length;
        var rawData = new byte[length + 4];

        rawData[0] = 0xA4;
        rawData[1] = length;
        rawData[2] = MessageId;

        writer.BaseStream.Position = 0;
        var read = writer.BaseStream.Read(payload, 0, payload.Length);
        if (read != payload.Length) throw new ArgumentException("Invalid payload length");

        for (var i = 0; i < length; i++) rawData[i + 3] = payload[i];

        rawData[length + 3] = rawData.CalculateChecksum();
        return rawData;
    }

    /// <inheritdoc />
    public abstract BinaryWriter EncodePayload();
}