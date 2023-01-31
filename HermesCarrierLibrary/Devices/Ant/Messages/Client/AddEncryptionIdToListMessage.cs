namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class AddEncryptionIdToListMessage : AntMessage
{
    /// <inheritdoc />
    public AddEncryptionIdToListMessage() : base(0x59, 6)
    {
    }

    /// <inheritdoc />
    public AddEncryptionIdToListMessage(byte channelNumber,
        byte[] encryptionId,
        byte listIndex) : this()
    {
        ChannelNumber = channelNumber;
        EncryptionId = encryptionId;
        ListIndex = listIndex;
    }

    public byte[] EncryptionId { get; set; }
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
        payload.Write(EncryptionId);
        payload.Write(ListIndex);
        return payload;
    }
}