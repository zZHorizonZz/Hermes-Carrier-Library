namespace HermesLibrary.Devices.Ant.Messages.Client;

public class CloseChannelMessage : BasicAntMessage
{
    /// <inheritdoc />
    public CloseChannelMessage() : base(0x4c, 1)
    {
    }

    public CloseChannelMessage(byte channelNumber) : this()
    {
        ChannelNumber = channelNumber;
    }

    public byte ChannelNumber { get; set; }

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
        return payload;
    }
}