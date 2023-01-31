using HermesCarrierLibrary.Devices.Ant.Enum;

namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class SetUsbDescriptorStringMessage : AntMessage
{
    public StringNumber StringNumber { get; set; }
    public string String { get; set; }

    /// <inheritdoc />
    public SetUsbDescriptorStringMessage() : base(0xC7, 64)
    {
    }

    /// <inheritdoc />
    public SetUsbDescriptorStringMessage(StringNumber stringNumber, string s) : this()
    {
        StringNumber = stringNumber;
        String = s;
    }

    /// <inheritdoc />
    public override void DecodePayload(BinaryReader payload)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override BinaryWriter EncodePayload()
    {
        if (StringNumber == StringNumber.PIDVID && String.Length != 4)
        {
            throw new OverflowException("StringNumber.PIDVID must be 4 characters long");
        }

        var payload = new BinaryWriter(new MemoryStream());
        payload.Write((byte)StringNumber);
        payload.Write(String);
        return payload;
    }
}