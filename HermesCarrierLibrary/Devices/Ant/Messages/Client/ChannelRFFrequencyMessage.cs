namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class ChannelRFFrequencyMessage : AntMessage
{
    public ChannelRFFrequencyMessage() : base(0x45, 2)
    {
    }

    public ChannelRFFrequencyMessage(byte rfFrequency) : this()
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