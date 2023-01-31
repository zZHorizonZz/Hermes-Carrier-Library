namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class ConfigureAdvancedBurstMessage : AntMessage
{
    public bool Enable { get; set; }
    public byte MaxPacketLength { get; set; }
    public ulong RequiredFeatures { get; set; }
    public ulong OptionalFeatures { get; set; }
    public byte StallCount { get; set; }
    public byte RetryCountExtension { get; set; }

    /// <inheritdoc />
    public ConfigureAdvancedBurstMessage() : base(0x78, 12)
    {
    }

    /// <inheritdoc />
    public ConfigureAdvancedBurstMessage(bool enable,
        byte maxPacketLength,
        ulong requiredFeatures,
        ulong optionalFeatures,
        byte stallCount,
        byte retryCountExtension) : this()
    {
        Enable = enable;
        MaxPacketLength = maxPacketLength;
        RequiredFeatures = requiredFeatures;
        OptionalFeatures = optionalFeatures;
        StallCount = stallCount;
        RetryCountExtension = retryCountExtension;
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
        payload.Write(0x00); //Filler
        payload.Write(Enable);
        payload.Write(MaxPacketLength);
        payload.Write(RequiredFeatures);
        payload.Write(OptionalFeatures);
        payload.Write(StallCount);
        payload.Write(RetryCountExtension);
        return payload;
    }
}