namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class SetChannelTransmitPowerMessage : AntMessage
{
    public byte TransmitPower { get; set; }

    /// <inheritdoc />
    public SetChannelTransmitPowerMessage() : base(0x60, 2)
    {
    }

    /// <inheritdoc />
    public SetChannelTransmitPowerMessage(byte transmitPower) : this()
    {
        TransmitPower = transmitPower;
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
        payload.Write(TransmitPower);
        return payload;
    }
}