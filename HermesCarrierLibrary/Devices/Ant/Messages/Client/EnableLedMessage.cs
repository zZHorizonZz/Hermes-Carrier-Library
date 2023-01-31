namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class EnableLedMessage : AntMessage
{
    /// <inheritdoc />
    public EnableLedMessage() : base(0x68, 2)
    {
    }

    /// <inheritdoc />
    public EnableLedMessage(bool enable) : this()
    {
        Enable = enable;
    }

    public bool Enable { get; set; }

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