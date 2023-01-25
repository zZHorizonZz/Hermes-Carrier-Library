namespace HermesLibrary.Devices.Ant.Messages;

public class AdvancedAntMessage
{
    public AdvancedAntMessage(byte[] data)
    {
        RawData = data;
        Sync = data[0];
        Length = data[1];
        MessageId = data[2];
        Channel = data[3];
        Payload = new byte[Length];
        Array.Copy(data, 4, Payload, 0, Length - 4);
    }

    public AdvancedAntMessage(byte messageId, byte channel, byte[] payload)
    {
        MessageId = messageId;
        Channel = channel;
        Payload = payload;
        Length = (byte)payload.Length;
        RawData = new byte[Length + 4];
        RawData[0] = 0xA4;
        RawData[1] = Length;
        RawData[2] = MessageId;
        RawData[3] = Channel;
        Array.Copy(Payload, 0, RawData, 4, Length);
    }

    public byte Length { get; set; }
    public byte Channel { get; set; }
    public byte MessageId { get; set; }
    public byte[] Payload { get; set; }
    public byte[] RawData { get; set; }
    public byte Sync { get; set; }

    public byte[] CreateRawMessage()
    {
        var checksum = GetChecksum();
        var rawMessage = new byte[RawData.Length + 1];
        Array.Copy(RawData, rawMessage, RawData.Length);
        rawMessage[RawData.Length] = checksum;
        return rawMessage;
    }

    public bool IsValid()
    {
        var checksum = 0;
        for (var i = 0; i < Length - 1; i++) checksum ^= RawData[i];

        return checksum == 0;
    }

    public byte GetChecksum()
    {
        var checksum = 0;
        for (var i = 0; i < Length; i++) checksum ^= RawData[i];

        return (byte)checksum;
    }

    public override string ToString()
    {
        return $"Sync: {Sync}," +
               $" Length: {Length}," +
               $" MessageId: {MessageId}," +
               $" Channel: {Channel}," +
               $" Payload: {BitConverter.ToString(Payload)}" +
               $" Checksum: {GetChecksum()}" +
               $" RawData: {BitConverter.ToString(RawData)}";
    }
}