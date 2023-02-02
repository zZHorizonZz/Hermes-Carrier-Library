namespace HermesCarrierLibrary.Devices.Ant.Messages.Device;

public class UserNvmMessage : AntMessage
{
    /// <inheritdoc />
    public UserNvmMessage() : base(0x7C, 64)
    {
    }

    public byte[] Data { get; set; }

    /// <inheritdoc />
    public override void DecodePayload(BinaryReader payload)
    {
        Data = payload.ReadBytes(Length - 1);
    }

    /// <inheritdoc />
    public override BinaryWriter EncodePayload()
    {
        throw new NotImplementedException();
    }
}