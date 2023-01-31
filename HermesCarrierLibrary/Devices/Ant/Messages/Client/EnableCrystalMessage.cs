namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class EnableCrystalMessage : AntMessage
{
    public bool Enable { get; set; }

    /// <inheritdoc />
    public EnableCrystalMessage() : base(0x6D, 1)
    {
    }

    /// <inheritdoc />
    public EnableCrystalMessage(bool enable) : this()
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
        payload.Write(Enable);
        return payload;
    }
}