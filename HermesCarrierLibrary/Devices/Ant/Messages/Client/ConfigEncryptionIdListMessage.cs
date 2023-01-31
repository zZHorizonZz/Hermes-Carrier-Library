namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class ConfigEncryptionIdListMessage : AntMessage
{
    public byte ListSize { get; set; }
    public byte ListType { get; set; }

    /// <inheritdoc />
    public ConfigEncryptionIdListMessage() : base(0x5a, 3)
    {
    }

    /// <inheritdoc />
    public ConfigEncryptionIdListMessage(byte listSize, byte listType) : this()
    {
        ListSize = listSize;
        ListType = listType;
    }

    /// <inheritdoc />
    public override void DecodePayload(BinaryReader payload)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override BinaryWriter EncodePayload()
    {
        var payload = new BinaryWriter(new MemoryStream());
        payload.Write(ChannelNumber);
        payload.Write(ListSize);
        payload.Write(ListType);
        return payload;
    }
}