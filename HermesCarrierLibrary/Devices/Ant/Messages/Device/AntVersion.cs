using System.Text;

namespace HermesCarrierLibrary.Devices.Ant.Messages.Device;

public class AntVersionMessage : AntMessage
{
    /// <inheritdoc />
    public AntVersionMessage() : base(0x3E, 1)
    {
    }

    public string Version { get; private set; }

    /// <inheritdoc />
    public override void DecodePayload(BinaryReader payload)
    {
        var builder = new StringBuilder();
        for (var i = 0; i < payload.BaseStream.Length; i++) builder.Append((char)payload.ReadByte());

        Version = builder.ToString();
    }

    /// <inheritdoc />
    public override BinaryWriter EncodePayload()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return nameof(AntVersionMessage) + ": " + Version;
    }
}