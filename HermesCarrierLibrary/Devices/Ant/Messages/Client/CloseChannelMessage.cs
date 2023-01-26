namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class CloseChannelMessage : AntMessage
{
    /// <inheritdoc />
    public CloseChannelMessage() : base(0x4c, 1)
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