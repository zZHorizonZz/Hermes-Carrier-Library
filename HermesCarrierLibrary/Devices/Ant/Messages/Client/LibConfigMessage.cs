using HermesCarrierLibrary.Devices.Ant.Enum;

namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class LibConfigMessage : AntMessage
{
    /// <inheritdoc />
    public LibConfigMessage() : base(0x6E, 2)
    {
    }

    /// <inheritdoc />
    public LibConfigMessage(LibConfig libConfig) : this()
    {
        LibConfig = libConfig;
    }

    public LibConfig LibConfig { get; set; } = LibConfig.Disabled;

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
        payload.Write((byte)LibConfig);
        return payload;
    }
}