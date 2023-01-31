namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class SetEncryptionInfoMessage : AntMessage
{
    public byte Parameters { get; set; }
    public byte[] DataString { get; set; }

    /// <inheritdoc />
    public SetEncryptionInfoMessage() : base(0x7F, 20)
    {
    }

    /// <inheritdoc />
    public SetEncryptionInfoMessage(byte parameters, byte[] dataString) : this()
    {
        Parameters = parameters;
        DataString = dataString;
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
        payload.Write(Parameters);
        payload.Write(DataString);
        return payload;
    }
}