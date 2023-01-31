namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class ConfigureSelectiveDataUpdatesMessage : AntMessage
{
    public byte SelectedData { get; set; }

    /// <inheritdoc />
    public ConfigureSelectiveDataUpdatesMessage() : base(0x7A, 2)
    {
    }

    /// <inheritdoc />
    public ConfigureSelectiveDataUpdatesMessage(byte selectedData) : this()
    {
        SelectedData = selectedData;
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
        payload.Write(SelectedData);
        return payload;
    }
}