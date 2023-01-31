namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class SearchWaveformMessage : AntMessage
{
    public ushort SearchWaveform { get; set; }

    /// <inheritdoc />
    public SearchWaveformMessage() : base(0x49, 3)
    {
    }

    /// <inheritdoc />
    public SearchWaveformMessage(ushort searchWaveform) : this()
    {
        SearchWaveform = searchWaveform;
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
        payload.Write(SearchWaveform);
        return payload;
    }
}