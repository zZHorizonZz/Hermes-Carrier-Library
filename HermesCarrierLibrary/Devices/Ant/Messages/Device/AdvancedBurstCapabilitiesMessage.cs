namespace HermesCarrierLibrary.Devices.Ant.Messages.Device;

public class AdvancedBurstCapabilitiesMessage : AntMessage
{
    /// <inheritdoc />
    public AdvancedBurstCapabilitiesMessage() : base(0x78, 5)
    {
    }

    public byte MessageType { get; set; }
    public byte SupportedMaxPacketLength { get; set; }
    public long SupportedFeatures { get; set; }

    /// <inheritdoc />
    public override void DecodePayload(BinaryReader payload)
    {
        MessageType = payload.ReadByte();
        SupportedMaxPacketLength = payload.ReadByte();
        SupportedFeatures = payload.ReadInt64();
    }

    /// <inheritdoc />
    public override BinaryWriter EncodePayload()
    {
        throw new NotImplementedException();
    }
}