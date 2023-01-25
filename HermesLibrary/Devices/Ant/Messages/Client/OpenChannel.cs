namespace HermesLibrary.Devices.Ant.Messages.Client;

public class OpenChannelMessage : BasicAntMessage
{
    /// <inheritdoc />
    public OpenChannelMessage() : base(0x4B, 1)
    {
    }

    public OpenChannelMessage(byte channelNumber) : this()
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