namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class SerialNumberSetChannelIdMessage : AntMessage
{
    public bool PairingRequest { get; set; }
    public byte DeviceTypeId { get; set; }
    public byte TransmissionType { get; set; }

    /// <inheritdoc />
    public SerialNumberSetChannelIdMessage() : base(0x65, 3)
    {
    }

    /// <inheritdoc />
    public SerialNumberSetChannelIdMessage(bool pairingRequest, byte deviceTypeId, byte transmissionType) : this()
    {
        PairingRequest = pairingRequest;
        DeviceTypeId = deviceTypeId;
        TransmissionType = transmissionType;
    }

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
        payload.Write((byte)(DeviceTypeId | (PairingRequest ? 0x80 : 0x00)));
        payload.Write(TransmissionType);
        return payload;
    }
}