namespace HermesLibrary.Devices.Ant.Util;

public static class MessageUtil
{
    public static byte CalculateChecksum(this byte[] message)
    {
        return CalculateChecksum(message, 0, message.Length - 1);
    }

    public static byte CalculateChecksum(this byte[] message, int startIndex, int length)
    {
        var checksum = message[startIndex];
        for (var i = startIndex + 1; i < startIndex + length; i++) checksum ^= message[i];

        return checksum;
    }
}