namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class UnAssignChannelMessage : AntMessage
{
    /// <inheritdoc />
    public UnAssignChannelMessage() : base(0x41, 1)
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