namespace HermesLibrary.Devices.Ant.Messages.Client;

public class UnAssignChannelMessage : BasicAntMessage
{
    /// <inheritdoc />
    public UnAssignChannelMessage() : base(0x41, 1)
    {
    }

    public UnAssignChannelMessage(byte channelNumber) : this()
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