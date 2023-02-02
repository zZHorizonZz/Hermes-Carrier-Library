namespace HermesCarrierLibrary.Devices.Ant.Messages.Device;

public class ChannelIdMessage : AntMessage
{
    /// <inheritdoc />
    public ChannelIdMessage() : base(0x51, 5)
    {
    }

    public ushort DeviceNumber { get; set; }
    public byte DeviceType { get; set; }
    public byte TransmissionType { get; set; }

    /// <inheritdoc />
    public override void DecodePayload(BinaryReader payload)
    {
        ChannelNumber = payload.ReadByte();
        DeviceNumber = payload.ReadUInt16();
        DeviceType = payload.ReadByte();
        TransmissionType = payload.ReadByte();
    }

    /// <inheritdoc />
    public override BinaryWriter EncodePayload()
    {
        throw new NotImplementedException();
    }
}