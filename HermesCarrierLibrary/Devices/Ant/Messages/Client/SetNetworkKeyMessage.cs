namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class SetNetworkKeyMessage : AntMessage
{
    /// <inheritdoc />
    public SetNetworkKeyMessage() : base(0x46, 9)
    {
    }

    /// <inheritdoc />
    public SetNetworkKeyMessage(byte network, byte[] key) : this()
    {
        NetworkNumber = network;
        NetworkKey = key;
    }

    public byte NetworkNumber { get; set; }
    public byte[] NetworkKey { get; set; }

    /// <inheritdoc />
    public override void DecodePayload(BinaryReader payload)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override BinaryWriter EncodePayload()
    {
        var payload = new BinaryWriter(new MemoryStream());
        payload.Write(NetworkNumber);
        payload.Write(NetworkKey);
        return payload;
    }
}