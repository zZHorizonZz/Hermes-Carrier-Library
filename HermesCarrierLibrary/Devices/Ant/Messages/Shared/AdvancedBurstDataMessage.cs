namespace HermesCarrierLibrary.Devices.Ant.Messages.Shared;

public class AdvancedBurstDataMessage : AntMessage
{
    public byte SequenceNumber { get; set; }
    public byte[] Data { get; set; }

    /// <inheritdoc />
    public AdvancedBurstDataMessage() : base(0x72, 64)
    {
    }

    /// <inheritdoc />
    public override void DecodePayload(BinaryReader payload)
    {
        SequenceNumber = payload.ReadByte();
        ChannelNumber = payload.ReadByte();

        Data = payload.ReadBytes(Length - 2);
    }

    /// <inheritdoc />
    public override BinaryWriter EncodePayload()
    {
        var payload = new BinaryWriter(new MemoryStream());
        payload.Write(SequenceNumber);
        payload.Write(ChannelNumber);
        payload.Write(Data);
        return payload;
    }
}