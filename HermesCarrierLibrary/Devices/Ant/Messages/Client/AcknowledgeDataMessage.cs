namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class AcknowledgedDataMessage : AntMessage
{
    public AcknowledgedDataMessage() : base(0x4f, 9)
    {
    }

    public AcknowledgedDataMessage(byte[] data) : this()
    {
        Data = data;
    }

    public byte[] Data { get; set; }

    /// <inheritdoc />
    public override void DecodePayload(BinaryReader payload)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override BinaryWriter EncodePayload()
    {
        var writer = new BinaryWriter(new MemoryStream());
        writer.Write(ChannelNumber);
        writer.Write(Data);
        return writer;
    }
}