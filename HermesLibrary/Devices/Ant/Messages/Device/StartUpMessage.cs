namespace HermesLibrary.Devices.Ant.Messages.Device;

public class StartUpMessage : BasicAntMessage
{
    /// <inheritdoc />
    public StartUpMessage() : base(0x6F, 1)
    {
    }

    public bool PowerOnReset { get; private set; }
    public bool HardwareResetLine { get; private set; }
    public bool WatchdogReset { get; private set; }
    public bool CommandReset { get; private set; }
    public bool SoftwareReset { get; private set; }
    public bool SuspendReset { get; private set; }

    /// <inheritdoc />
    public override void DecodePayload(BinaryReader payload)
    {
        var resetFlags = payload.ReadByte();
        if (resetFlags == 0)
        {
            PowerOnReset = true;
        }
        else
        {
            HardwareResetLine = (resetFlags & 0x01) != 0;
            WatchdogReset = (resetFlags & 0x02) != 0;
            CommandReset = (resetFlags & 0x04) != 0;
            SoftwareReset = (resetFlags & 0x08) != 0;
            SuspendReset = (resetFlags & 0x10) != 0;
        }
    }

    /// <inheritdoc />
    public override BinaryWriter EncodePayload()
    {
        throw new NotImplementedException();
    }
}