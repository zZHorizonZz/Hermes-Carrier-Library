namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class ProximitySearchMessage : AntMessage
{
    public byte UcSearchThreshold { get; set; }

    /// <inheritdoc />
    public ProximitySearchMessage() : base(0x71, 2)
    {
    }

    /// <inheritdoc />
    public ProximitySearchMessage(byte ucSearchThreshold) : this()
    {
        UcSearchThreshold = ucSearchThreshold;
    }

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
        payload.Write(UcSearchThreshold);
        return payload;
    }
}