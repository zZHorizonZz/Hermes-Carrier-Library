namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class AddChannelIdToListMessage : AntMessage
{
    /// <inheritdoc />
    public AddChannelIdToListMessage() : base(0x59, 6)
    {
    }

    /// <inheritdoc />
    public AddChannelIdToListMessage(byte channelNumber,
        ushort deviceNumber,
        byte deviceType,
        byte transmissionType,
        byte listIndex) : this()
    {
        ChannelNumber = channelNumber;
        DeviceNumber = deviceNumber;
        DeviceType = deviceType;
        TransmissionType = transmissionType;
        ListIndex = listIndex;
    }

    public ushort DeviceNumber { get; set; }
    public byte DeviceType { get; set; }
    public byte TransmissionType { get; set; }
    public byte ListIndex { get; set; }

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
        payload.Write(DeviceType);
        payload.Write(TransmissionType);
        payload.Write(ListIndex);
        return payload;
    }
}