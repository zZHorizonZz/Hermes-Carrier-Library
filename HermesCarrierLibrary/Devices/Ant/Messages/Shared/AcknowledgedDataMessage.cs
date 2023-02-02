namespace HermesCarrierLibrary.Devices.Ant.Messages.Shared;

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
        ChannelNumber = payload.ReadByte();
        Data = payload.ReadBytes(8);
    }

    /// <inheritdoc />
    public override BinaryWriter EncodePayload()
    {
        var writer = new BinaryWriter(new MemoryStream());
        writer.Write(ChannelNumber);
        writer.Write(Data);
        return writer;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"AcknowledgedDataMessage: ChannelNumber={ChannelNumber}, Data={BitConverter.ToString(Data)}";
    }
}