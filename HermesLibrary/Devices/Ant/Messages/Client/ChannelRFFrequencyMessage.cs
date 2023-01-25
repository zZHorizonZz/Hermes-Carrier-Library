namespace HermesLibrary.Devices.Ant.Messages.Client;

public class ChannelRFFrequencyMessage : BasicAntMessage
{
    public ChannelRFFrequencyMessage() : base(0x45, 2)
    {
    }

    public ChannelRFFrequencyMessage(byte channelNumber, byte rfFrequency) : this()
    {
        ChannelNumber = channelNumber;
        RFFrequency = rfFrequency;
    }

    public byte ChannelNumber { get; set; }
    public byte RFFrequency { get; set; }

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
        payload.Write(RFFrequency);
        return payload;
    }
}