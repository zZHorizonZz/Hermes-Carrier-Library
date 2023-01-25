namespace HermesLibrary.Devices.Ant.Enum;

public class AntReceiverMessageType
{
    public static readonly AntReceiverMessageType UNKNOWN = new(-1);
    public static readonly AntReceiverMessageType RX_ANT_MESSAGE = new(1);
    public static readonly AntReceiverMessageType CHANNEL_DEATH = new(2);
    public static readonly AntReceiverMessageType BURST_STATE_CHANGE = new(101);
    public static readonly AntReceiverMessageType LIB_CONFIG_CHANGE = new(102);
    public static readonly AntReceiverMessageType BACKGROUND_SCAN_STATE_CHANGE = new(103);
    public static readonly AntReceiverMessageType EVENT_BUFFER_SETTINGS_CHANGE = new(104);

    private readonly int mRawValue;

    private AntReceiverMessageType(int rawValue)
    {
        mRawValue = rawValue;
    }

    public static IEnumerable<AntReceiverMessageType> Values => new[]
    {
        UNKNOWN,
        RX_ANT_MESSAGE,
        CHANNEL_DEATH,
        BURST_STATE_CHANGE,
        LIB_CONFIG_CHANGE,
        BACKGROUND_SCAN_STATE_CHANGE,
        EVENT_BUFFER_SETTINGS_CHANGE
    };

    public int GetRawValue()
    {
        return mRawValue;
    }

    public static AntReceiverMessageType Create(int rawValue)
    {
        var code = UNKNOWN;
        foreach (var t in Values)
        {
            if (!t.Equals(rawValue)) continue;

            code = t;
            break;
        }

        return code;
    }

    private bool Equals(int rawValue)
    {
        return rawValue == mRawValue;
    }
}