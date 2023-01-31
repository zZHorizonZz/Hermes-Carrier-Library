namespace HermesCarrierLibrary.Devices.Ant.Messages.Device;

public class SelectiveDataUpdateMaskSettingMessage : AntMessage
{
    public byte SubMessageId { get; set; }
    public byte[] SduMask { get; set; }

    /// <inheritdoc />
    public SelectiveDataUpdateMaskSettingMessage() : base(0x7B, 9)
    {
    }

    /// <inheritdoc />
    public override void DecodePayload(BinaryReader payload)
    {
        SubMessageId = payload.ReadByte();
        SduMask = payload.ReadBytes(8);
    }

    /// <inheritdoc />
    public override BinaryWriter EncodePayload()
    {
        throw new NotImplementedException();
    }
}