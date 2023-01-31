namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class ChannelSearchPriorityMessage : AntMessage
{
    /// <inheritdoc />
    public ChannelSearchPriorityMessage() : base(0x75, 2)
    {
    }

    /// <inheritdoc />
    public ChannelSearchPriorityMessage(byte searchPriority) : this()
    {
        SearchPriority = searchPriority;
    }

    public byte SearchPriority { get; set; }

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
        payload.Write(SearchPriority);
        return payload;
    }
}