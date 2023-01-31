namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class SearchTimeoutMessage : AntMessage
{
    /// <inheritdoc />
    public SearchTimeoutMessage() : base(0x44, 1)
    {
    }

    public SearchTimeoutMessage(byte searchTimeout) : this()
    {
        SearchTimeout = searchTimeout;
    }

    public byte SearchTimeout { get; set; }

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
        payload.Write(SearchTimeout);
        return payload;
    }
}