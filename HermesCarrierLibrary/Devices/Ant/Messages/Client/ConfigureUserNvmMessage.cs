namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class ConfigureUserNvmMessage : AntMessage
{
    public ushort Address { get; set; }
    public byte[] Data { get; set; }

    /// <inheritdoc />
    public ConfigureUserNvmMessage() : base(0x7C, 2)
    {
    }

    /// <inheritdoc />
    public ConfigureUserNvmMessage(ushort address, byte[] data) : this()
    {
        Address = address;
        Data = data;
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
        payload.Write(0x00); // Filler byte
        payload.Write(Address);
        payload.Write(Data);
        return payload;
    }
}