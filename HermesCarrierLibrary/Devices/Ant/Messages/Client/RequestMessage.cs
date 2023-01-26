using HermesCarrierLibrary.Devices.Ant.Enum;

namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class RequestMessage : AntMessage
{
    /// <inheritdoc />
    public RequestMessage() : base(0x4D, 2)
    {
    }

    /// <inheritdoc />
    public RequestMessage(RequestMessageType requestType) : this()
    {
        RequestType = requestType;
    }

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