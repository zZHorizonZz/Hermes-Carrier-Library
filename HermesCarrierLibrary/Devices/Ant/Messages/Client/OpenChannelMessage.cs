namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class OpenChannelMessage : AntMessage
{
    /// <inheritdoc />
    public OpenChannelMessage() : base(0x4B, 1)
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
        payload.Write(ChannelNumber);
        return payload;
    }
}