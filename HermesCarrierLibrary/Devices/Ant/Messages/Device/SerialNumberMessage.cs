namespace HermesCarrierLibrary.Devices.Ant.Messages.Device;

public class SerialNumberMessage : AntMessage
{
    /// <inheritdoc />
    public SerialNumberMessage() : base(0x61, 4)
    {
    }

    public byte[] SerialNumber { get; set; }

    /// <inheritdoc />
    public override void DecodePayload(BinaryReader payload)
    {
        SerialNumber = payload.ReadBytes(4);
    }

    /// <inheritdoc />
    public override BinaryWriter EncodePayload()
    {
        throw new NotImplementedException();
    }
}