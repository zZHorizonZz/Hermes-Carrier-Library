namespace HermesCarrierLibrary.Devices.Ant.Messages.Shared;

public class BroadcastDataMessage : AntMessage
{
    public BroadcastDataMessage() : base(0x4e, 9)
    {
    }

    public BroadcastDataMessage(byte[] data) : this()
    {
        Data = data;
    }

    public byte[] Data { get; set; }

    /// <inheritdoc />
    public override void DecodePayload(BinaryReader payload)
    {
        ChannelNumber = payload.ReadByte();
        Data = payload.ReadBytes(8);
    }

    /// <inheritdoc />
    public override BinaryWriter EncodePayload()
    {
        var writer = new BinaryWriter(new MemoryStream());
        writer.Write(ChannelNumber);
        writer.Write(Data);
        return writer;
    }
}