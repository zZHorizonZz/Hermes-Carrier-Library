namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class FrequencyAgilityMessage : AntMessage
{
    /// <inheritdoc />
    public FrequencyAgilityMessage() : base(0x70, 4)
    {
    }

    /// <inheritdoc />
    public FrequencyAgilityMessage(byte ucFrequency1, byte ucFrequency2, byte ucFrequency3) : this()
    {
        UcFrequency1 = ucFrequency1;
        UcFrequency2 = ucFrequency2;
        UcFrequency3 = ucFrequency3;
    }

    public byte UcFrequency1 { get; set; }
    public byte UcFrequency2 { get; set; }
    public byte UcFrequency3 { get; set; }

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
        payload.Write(UcFrequency1);
        payload.Write(UcFrequency2);
        payload.Write(UcFrequency3);
        return payload;
    }
}