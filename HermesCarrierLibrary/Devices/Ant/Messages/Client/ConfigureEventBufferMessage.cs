namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class ConfigureEventBufferMessage : AntMessage
{
    public byte Config { get; set; }
    public ushort Size { get; set; }
    public ushort Time { get; set; }

    /// <inheritdoc />
    public ConfigureEventBufferMessage() : base(0x74, 6)
    {
    }

    /// <inheritdoc />
    public ConfigureEventBufferMessage(byte config, ushort size, ushort time) : this()
    {
        Config = config;
        Size = size;
        Time = time;
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
        payload.Write(Config);
        payload.Write(Size);
        payload.Write(Time);
        return payload;
    }
}