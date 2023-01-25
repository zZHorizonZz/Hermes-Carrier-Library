using HermesLibrary.Devices.Ant.Enum;

namespace HermesLibrary.Devices.Ant.Messages.Client;

public class RequestMessage : BasicAntMessage
{
    /// <inheritdoc />
    public RequestMessage() : base(0x4D, 2)
    {
    }

    /// <inheritdoc />
    public RequestMessage(byte channelNumber, RequestMessageType requestType) : this()
    {
        ChannelNumber = channelNumber;
        RequestType = requestType;
    }

    public byte ChannelNumber { get; set; }
    public RequestMessageType RequestType { get; set; }

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
        payload.Write((byte)RequestType);
        return payload;
    }
}