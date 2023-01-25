namespace HermesLibrary.Devices.Ant.Messages.Client;

public class BroadcastDataMessage : BasicAntMessage
{
    public BroadcastDataMessage() : base(0x4e, 9)
    {
    }

    public BroadcastDataMessage(byte channelNumber, byte[] data) : this()
    {
        ChannelNumber = channelNumber;
        Data = data;
    }

    public byte ChannelNumber { get; set; }
    public byte[] Data { get; set; }

    /// <inheritdoc />
    public override void DecodePayload(BinaryReader payload)
    {
        throw new NotImplementedException();
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