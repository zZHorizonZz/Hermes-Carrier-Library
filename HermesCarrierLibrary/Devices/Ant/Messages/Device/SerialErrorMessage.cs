namespace HermesCarrierLibrary.Devices.Ant.Messages.Device;

public class SerialErrorMessage : AntMessage
{
    public byte ErrorNumber { get; set; }

    /// <inheritdoc />
    public SerialErrorMessage() : base(0xAE, 1)
    {
    }

    /// <inheritdoc />
    public override void DecodePayload(BinaryReader payload)
    {
        ErrorNumber = payload.ReadByte();
    }

    /// <inheritdoc />
    public override BinaryWriter EncodePayload()
    {
        throw new NotImplementedException();
    }
}