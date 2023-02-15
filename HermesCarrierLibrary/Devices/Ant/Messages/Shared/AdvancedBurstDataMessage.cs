namespace HermesCarrierLibrary.Devices.Ant.Messages.Shared;

public class AdvancedBurstDataMessage : AntMessage
{
    /// <inheritdoc />
    public AdvancedBurstDataMessage() : base(0x72, 64)
    {
    }

    public byte SequenceNumber { get; set; }
    public byte[] Data { get; set; }

    /// <inheritdoc />
    public override void DecodePayload(BinaryReader payload)
    {
        var flags = payload.ReadByte();
        SequenceNumber = (byte)(flags >> 5);
        ChannelNumber = (byte)(flags & 0x1F);

        Data = payload.ReadBytes(Length - 2);
    }

    /// <inheritdoc />
    public override BinaryWriter EncodePayload()
    {
        var payload = new BinaryWriter(new MemoryStream());
        
        var flags = (byte)0;
        flags |= (byte)(SequenceNumber << 5);
        flags |= ChannelNumber;
        
        payload.Write(flags);
        payload.Write(Data);
        return payload;
    }
}