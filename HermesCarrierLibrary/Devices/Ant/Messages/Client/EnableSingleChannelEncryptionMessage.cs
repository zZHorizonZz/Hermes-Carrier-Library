namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class EnableSingleChannelEncryptionMessage : AntMessage
{
    /// <inheritdoc />
    public EnableSingleChannelEncryptionMessage() : base(0x7D, 4)
    {
    }

    /// <inheritdoc />
    public EnableSingleChannelEncryptionMessage(byte encryptionMode, byte volatileKeyIndex, byte decimationRate) :
        this()
    {
        EncryptionMode = encryptionMode;
        VolatileKeyIndex = volatileKeyIndex;
        DecimationRate = decimationRate;
    }

    public byte EncryptionMode { get; set; }
    public byte VolatileKeyIndex { get; set; }
    public byte DecimationRate { get; set; }

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
        payload.Write(EncryptionMode);
        payload.Write(VolatileKeyIndex);
        payload.Write(DecimationRate);
        return payload;
    }
}