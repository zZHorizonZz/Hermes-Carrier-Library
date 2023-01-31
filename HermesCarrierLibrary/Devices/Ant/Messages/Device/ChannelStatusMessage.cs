namespace HermesCarrierLibrary.Devices.Ant.Messages.Device;

public class ChannelStatusMessage : AntMessage
{
    public byte ChannelStatus { get; set; }

    /// <inheritdoc />
    public ChannelStatusMessage() : base(0x52, 2)
    {
    }

    /// <inheritdoc />
    public override void DecodePayload(BinaryReader payload)
    {
        ChannelNumber = payload.ReadByte();
        ChannelStatus = payload.ReadByte();
    }

    /// <inheritdoc />
    public override BinaryWriter EncodePayload()
    {
        throw new NotImplementedException();
    }
}