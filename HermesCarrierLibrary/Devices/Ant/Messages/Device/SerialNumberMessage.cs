namespace HermesCarrierLibrary.Devices.Ant.Messages.Device;

public class SerialNumberMessage : AntMessage
{
    public byte[] SerialNumber { get; set; }

    /// <inheritdoc />
    public SerialNumberMessage() : base(0x61, 4)
    {
    }

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