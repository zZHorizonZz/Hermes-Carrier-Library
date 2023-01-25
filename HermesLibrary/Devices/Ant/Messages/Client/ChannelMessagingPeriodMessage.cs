namespace HermesLibrary.Devices.Ant.Messages.Client;

public class ChannelMessagingPeriodMessage : BasicAntMessage
{
    /// <inheritdoc />
    public ChannelMessagingPeriodMessage() : base(0x43, 2)
    {
    }

    public ChannelMessagingPeriodMessage(byte channelNumber, ushort messagingPeriod) : this()
    {
        ChannelNumber = channelNumber;
        MessagingPeriod = messagingPeriod;
    }

    public byte ChannelNumber { get; set; }
    public ushort MessagingPeriod { get; set; }

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
        payload.Write(MessagingPeriod);
        return payload;
    }
}