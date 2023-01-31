namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class CwTestMessage : AntMessage
{
    public byte TransmitPower { get; set; }
    public byte ChannelRfFrequency { get; set; }

    /// <inheritdoc />
    public CwTestMessage() : base(0x48, 3)
    {
    }

    /// <inheritdoc />
    public CwTestMessage(byte transmitPower, byte channelRfFrequency) : this()
    {
        TransmitPower = transmitPower;
        ChannelRfFrequency = channelRfFrequency;
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
        payload.Write(TransmitPower);
        payload.Write(ChannelRfFrequency);
        return payload;
    }
}