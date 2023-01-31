namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class EnableExtRxMessagesMessage : AntMessage
{
    public bool Enable { get; set; }

    /// <inheritdoc />
    public EnableExtRxMessagesMessage() : base(0x66, 2)
    {
    }

    /// <inheritdoc />
    public EnableExtRxMessagesMessage(bool enable) : this()
    {
        Enable = enable;
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
        payload.Write(Enable);
        return payload;
    }
}