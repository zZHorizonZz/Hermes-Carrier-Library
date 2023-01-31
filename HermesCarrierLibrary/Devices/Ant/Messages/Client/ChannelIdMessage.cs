namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class ChannelIdMessage : AntMessage
{
    /// <inheritdoc />
    public ChannelIdMessage() : base(0x51, 5)
    {
    }

    /// <inheritdoc />
    public ChannelIdMessage(ushort device, bool pairing, byte deviceType, byte transmission) : this()
    {
        DeviceNumber = device;
        Pairing = pairing;
        DeviceType = deviceType;
        TransmissionType = transmission;
    }

    public ushort DeviceNumber { get; set; }

    public bool Pairing { get; set; }

    public byte DeviceType { get; set; }

    public byte TransmissionType { get; set; }

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
        payload.Write(DeviceNumber);
        var type = (byte)(DeviceType | (Pairing ? 0x80 : 0x00));
        payload.Write(type);
        payload.Write(TransmissionType);
        return payload;
    }
}