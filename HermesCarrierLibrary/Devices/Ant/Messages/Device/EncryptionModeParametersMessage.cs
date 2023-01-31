using HermesCarrierLibrary.Devices.Ant.Enum;

namespace HermesCarrierLibrary.Devices.Ant.Messages.Device;

public class EncryptionModeParametersMessage : AntMessage
{
    public EncryptionParameter EncryptionParameter { get; set; }
    public byte[] Data { get; set; }

    /// <inheritdoc />
    public EncryptionModeParametersMessage() : base(0x7D, 20)
    {
    }

    /// <inheritdoc />
    public override void DecodePayload(BinaryReader payload)
    {
        EncryptionParameter = (EncryptionParameter)payload.ReadByte();
        Data = EncryptionParameter switch
        {
            EncryptionParameter.MaxSupportedEncryptionMode => payload.ReadBytes(1),
            EncryptionParameter.EncryptionId => payload.ReadBytes(4),
            EncryptionParameter.UserInformationString => payload.ReadBytes(19),
            _ => Data
        };
    }

    /// <inheritdoc />
    public override BinaryWriter EncodePayload()
    {
        throw new NotImplementedException();
    }
}