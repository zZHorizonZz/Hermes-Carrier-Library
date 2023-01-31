namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class ChannelRfFrequencyMessage : AntMessage
{
    /// <inheritdoc />
    public ChannelRfFrequencyMessage() : base(0x45, 2)
    {
    }

    /// <inheritdoc />
    public ChannelRfFrequencyMessage(byte rfFrequency) : this()
    {
        RFFrequency = rfFrequency;
    }

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