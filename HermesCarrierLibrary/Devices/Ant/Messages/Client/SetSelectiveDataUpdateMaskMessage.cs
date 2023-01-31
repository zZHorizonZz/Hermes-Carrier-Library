namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class SetSelectiveDataUpdateMaskMessage : AntMessage
{
    /// <inheritdoc />
    public SetSelectiveDataUpdateMaskMessage() : base(0x7B, 9)
    {
    }

    /// <inheritdoc />
    public SetSelectiveDataUpdateMaskMessage(byte sduMaskNumber, byte[] sduMask) : this()
    {
        SduMaskNumber = sduMaskNumber;
        SduMask = sduMask;
    }

    public byte SduMaskNumber { get; set; }
    public byte[] SduMask { get; set; }

    /// <inheritdoc />
    public override void DecodePayload(BinaryReader payload)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override BinaryWriter EncodePayload()
    {
        var payload = new BinaryWriter(new MemoryStream());
        payload.Write(SduMaskNumber);
        payload.Write(SduMask);
        return payload;
    }
}