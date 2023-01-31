namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class ChannelSearchSharingMessage : AntMessage
{
    public byte SearchSharingCycles { get; set; }

    /// <inheritdoc />
    public ChannelSearchSharingMessage() : base(0x81, 2)
    {
    }

    /// <inheritdoc />
    public ChannelSearchSharingMessage(byte searchSharingCycles) : this()
    {
        SearchSharingCycles = searchSharingCycles;
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
        payload.Write(ChannelNumber);
        payload.Write(SearchSharingCycles);
        return payload;
    }
}