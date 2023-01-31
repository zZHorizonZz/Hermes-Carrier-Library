namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class HighDutySearchMessage : AntMessage
{
    public bool Enable { get; set; }
    public byte SuppressionCycle { get; set; } = 0x03;

    /// <inheritdoc />
    public HighDutySearchMessage() : base(0x77, 3)
    {
    }

    /// <inheritdoc />
    public HighDutySearchMessage(bool enable, byte suppressionCycle) : this()
    {
        Enable = enable;
        SuppressionCycle = suppressionCycle;
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
        payload.Write(Enable);
        payload.Write(SuppressionCycle);
        return payload;
    }
}