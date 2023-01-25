namespace HermesLibrary.Devices.Ant.Messages.Client;

public class OpenRxScanMode : BasicAntMessage
{
    /// <inheritdoc />
    public OpenRxScanMode() : base(0x5B, 1)
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