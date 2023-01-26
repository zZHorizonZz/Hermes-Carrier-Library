using Android.Runtime;
using Java.Nio;
using Byte = Java.Lang.Byte;
using Object = Java.Lang.Object;

namespace HermesCarrierLibrary.Platforms.Android.Devices.Util;

/// <summary>
///     Work around for faulty JNI wrapping in Xamarin library.  Fixes a bug
///     where binding for Java.Nio.ByteBuffer.Get(byte[], int, int) allocates a new temporary
///     Java byte array on every call
///     See https://bugzilla.xamarin.com/show_bug.cgi?id=31260
///     and http://stackoverflow.com/questions/30268400/xamarin-implementation-of-bytebuffer-get-wrong
/// </summary>
public static class BufferUtil
{
    private static nint _byteBufferClassRef;
    private static nint _byteBufferGetBii;

    public static ByteBuffer? Get(this ByteBuffer buffer, JavaArray<Byte> dst, int dstOffset, int byteCount)
    {
        if (_byteBufferClassRef == nint.Zero) _byteBufferClassRef = JNIEnv.FindClass("java/nio/ByteBuffer");

        if (_byteBufferGetBii == nint.Zero)
            _byteBufferGetBii = JNIEnv.GetMethodID(_byteBufferClassRef, "get", "([BII)Ljava/nio/ByteBuffer;");

        return Object.GetObject<ByteBuffer>(
            JNIEnv.CallObjectMethod(buffer.Handle, _byteBufferGetBii, new(dst), new(dstOffset), new(byteCount)),
            JniHandleOwnership.TransferLocalRef);
    }

    public static byte[]? ToByteArray(this ByteBuffer buffer)
    {
        var classHandle = JNIEnv.FindClass("java/nio/ByteBuffer");
        var methodId = JNIEnv.GetMethodID(classHandle, "array", "()[B");
        var resultHandle = JNIEnv.CallObjectMethod(buffer.Handle, methodId);

        var result = JNIEnv.GetArray<byte>(resultHandle);

        JNIEnv.DeleteLocalRef(resultHandle);

        return result;
    }

    public static byte[] ToByteArray(byte b)
    {
        return new[] { b };
    }

    public static byte[] ToByteArray(int i)
    {
        var array = new byte[4];

        array[3] = (byte)(i & 0xFF);
        array[2] = (byte)((i >> 8) & 0xFF);
        array[1] = (byte)((i >> 16) & 0xFF);
        array[0] = (byte)((i >> 24) & 0xFF);

        return array;
    }

    public static byte[] ToByteArray(short i)
    {
        var array = new byte[2];

        array[1] = (byte)(i & 0xFF);
        array[0] = (byte)((i >> 8) & 0xFF);

        return array;
    }
}