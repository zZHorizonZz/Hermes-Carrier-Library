namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class LowPrioritySearchTimeoutMessage : AntMessage
{
    /// <inheritdoc />
    public LowPrioritySearchTimeoutMessage() : base(0x63, 2)
    {
    }

    /// <inheritdoc />
    public LowPrioritySearchTimeoutMessage(byte searchTimeout) : this()
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