namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class ChannelMessagingPeriodMessage : AntMessage
{
    /// <inheritdoc />
    public ChannelMessagingPeriodMessage() : base(0x43, 2)
    {
    }

    public ChannelMessagingPeriodMessage(ushort messagingPeriod) : this()
    {
        MessagingPeriod = messagingPeriod;
    }

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