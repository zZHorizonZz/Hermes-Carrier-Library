namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class TransmitPowerMessage : AntMessage
{
    /// <inheritdoc />
    public TransmitPowerMessage() : base(0x47, 2)
    {
    }

    /// <inheritdoc />
    public TransmitPowerMessage(byte transmitPower) : this()
    {
        TransmitPower = transmitPower;
    }

    public byte TransmitPower { get; set; }

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
        return payload;
    }
}