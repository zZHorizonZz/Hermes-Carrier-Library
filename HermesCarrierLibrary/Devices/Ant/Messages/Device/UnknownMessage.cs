namespace HermesCarrierLibrary.Devices.Ant.Messages.Device;

public class UnknownMessage : AntMessage
{
    /// <inheritdoc />
    public UnknownMessage(byte[] data) : base(0xFF, byte.MaxValue)
    {
        Data = data;
    }

    public byte[] Data { get; }

    /// <inheritdoc />
    public override void DecodePayload(BinaryReader payload)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override BinaryWriter EncodePayload()
    {
        throw new NotImplementedException();
    }
}