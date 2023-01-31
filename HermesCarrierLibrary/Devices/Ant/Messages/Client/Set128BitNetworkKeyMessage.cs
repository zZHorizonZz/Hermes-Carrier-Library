namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class Set128BitNetworkKeyMessage : AntMessage
{
    /// <inheritdoc />
    public Set128BitNetworkKeyMessage() : base(0x76, 17)
    {
    }

    /// <inheritdoc />
    public Set128BitNetworkKeyMessage(byte[] networkKey) : this()
    {
        NetworkKey = networkKey;
    }

    public byte[] NetworkKey { get; set; }

    /// <inheritdoc />
    public override void DecodePayload(BinaryReader payload)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override BinaryWriter EncodePayload()
    {
        if (NetworkKey is not { Length: 16 }) throw new ArgumentException("NetworkKey must be 16 bytes long");

        var payload = new BinaryWriter(new MemoryStream());
        payload.Write(ChannelNumber);
        payload.Write(NetworkKey);
        return payload;
    }
}