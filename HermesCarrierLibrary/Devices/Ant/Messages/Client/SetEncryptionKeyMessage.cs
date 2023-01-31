namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class SetEncryptionKeyMessage : AntMessage
{
    /// <inheritdoc />
    public SetEncryptionKeyMessage() : base(0x7E, 17)
    {
    }

    /// <inheritdoc />
    public SetEncryptionKeyMessage(byte volatileKeyIndex, byte[] encryptionKey) : this()
    {
        VolatileKeyIndex = volatileKeyIndex;
        EncryptionKey = encryptionKey;
    }

    public byte VolatileKeyIndex { get; set; }
    public byte[] EncryptionKey { get; set; }

    /// <inheritdoc />
    public override void DecodePayload(BinaryReader payload)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override BinaryWriter EncodePayload()
    {
        var payload = new BinaryWriter(new MemoryStream());
        payload.Write(VolatileKeyIndex);
        payload.Write(EncryptionKey);
        return payload;
    }
}