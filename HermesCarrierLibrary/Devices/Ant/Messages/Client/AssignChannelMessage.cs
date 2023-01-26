using HermesCarrierLibrary.Devices.Ant.Channel;
using HermesCarrierLibrary.Devices.Ant.Enum;

namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class AssignChannelMessage : AntMessage
{
    /// <inheritdoc />
    public AssignChannelMessage() : base(0x42, 3)
    {
    }

    /// <inheritdoc />
    public AssignChannelMessage(ChannelType type, byte networkNumber,
        ExtendedAssignmentType extended = ExtendedAssignmentType.UNKNOWN) : base(0x42, 3)
    {
        ChannelType = type;
        NetworkNumber = networkNumber;
        ExtendedAssignmentType = extended;
    }

    public ChannelType ChannelType { get; set; }

    public byte NetworkNumber { get; set; }

    public ExtendedAssignmentType ExtendedAssignmentType { get; set; } = ExtendedAssignmentType.UNKNOWN;

    /// <inheritdoc />
    public override void DecodePayload(BinaryReader payload)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override BinaryWriter EncodePayload()
    {
        var payload = new BinaryWriter(new MemoryStream());
        payload.Write(ChannelNumber);
        payload.Write((byte)ChannelType);
        payload.Write(NetworkNumber);
        if (ExtendedAssignmentType != ExtendedAssignmentType.UNKNOWN)
            payload.Write((byte)ExtendedAssignmentType);
        return payload;
    }
}