namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class CwInitMessage : AntMessage
{
    /// <inheritdoc />
    public CwInitMessage() : base(0x53, 1)
    {
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
        payload.Write(0x00); // Filler byte
        return payload;
    }
}