namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class ConfigIdListMessage : AntMessage
{
    /// <inheritdoc />
    public ConfigIdListMessage() : base(0x5A, 3)
    {
    }

    /// <inheritdoc />
    public ConfigIdListMessage(byte listSize, byte exclude) : this()
    {
        ListSize = listSize;
        Exclude = exclude;
    }

    public byte ListSize { get; set; }
    public byte Exclude { get; set; }

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
        payload.Write(Exclude);
        return payload;
    }
}