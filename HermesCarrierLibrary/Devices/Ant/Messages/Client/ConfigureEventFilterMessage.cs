namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class ConfigureEventFilterMessage : AntMessage
{
    public ushort EventFilter { get; set; }

    /// <inheritdoc />
    public ConfigureEventFilterMessage() : base(0x79, 3)
    {
    }

    /// <inheritdoc />
    public ConfigureEventFilterMessage(ushort eventFilter) : this()
    {
        EventFilter = eventFilter;
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
        payload.Write(0x00); // Filler byte
        payload.Write(EventFilter);
        return payload;
    }
}