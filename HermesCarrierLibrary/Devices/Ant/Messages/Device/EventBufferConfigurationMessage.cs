namespace HermesCarrierLibrary.Devices.Ant.Messages.Device;

public class EventBufferConfigurationMessage : AntMessage
{
    public byte BufferConfig { get; set; }
    public ushort BufferSize { get; set; }
    public ushort BufferTime { get; set; }

    /// <inheritdoc />
    public EventBufferConfigurationMessage() : base(0x74, 6)
    {
    }

    /// <inheritdoc />
    public override void DecodePayload(BinaryReader payload)
    {
        payload.ReadByte(); // Skip channel number

        BufferConfig = payload.ReadByte();
        BufferSize = payload.ReadUInt16();
        BufferTime = payload.ReadUInt16();
    }

    /// <inheritdoc />
    public override BinaryWriter EncodePayload()
    {
        throw new NotImplementedException();
    }
}