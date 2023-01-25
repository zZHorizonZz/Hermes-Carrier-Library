namespace HermesLibrary.Devices.Ant.Messages.Client;

public class ResetSystemMessage : BasicAntMessage
{
    /// <inheritdoc />
    public ResetSystemMessage() : base(0x4A, 1)
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
        payload.Write((byte)0);
        return payload;
    }
}