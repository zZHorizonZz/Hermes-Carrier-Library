﻿using Android.Hardware.Usb;
using HermesCarrierLibrary.Devices.Usb;
using HermesCarrierLibrary.Platforms.Android.Devices.Util;
using Java.Nio;

namespace HermesCarrierLibrary.Platforms.Android.Devices;

public class DroidUsbRequest : IUsbRequest
{
    internal readonly UsbRequest Request;

    private ByteBuffer mBuffer;

    public DroidUsbRequest(UsbRequest usbRequest)
    {
        Request = usbRequest;
    }

    /// <summary>
    ///     This is a hack to get the buffer from the UsbRequest object.
    ///     Because of the conversion from Java to C# the buffer does not properly propagate the new data.
    /// </summary>
    public byte[] Buffer
    {
        get
        {
            var buffer = new byte[mBuffer.Capacity()];
            if (mBuffer == null) return null;

            System.Buffer.BlockCopy(mBuffer.ToByteArray(), 0, buffer, 0, buffer.Length);
            return buffer;
        }
    }

    /// <inheritdoc />
    public IUsbEndpoint Endpoint { get; }

    /// <inheritdoc />
    public bool Cancel()
    {
        return Request.Cancel();
    }

    /// <inheritdoc />
    public void Close()
    {
        Request.Close();
    }

    /// <inheritdoc />
    public bool Initialize(IUsbDevice device, IUsbEndpoint endpoint)
    {
        if (device is DroidUsbDevice usbDevice && endpoint is DroidUsbEndpoint usbEndpoint)
            return Request.Initialize(usbDevice.DeviceConnection, usbEndpoint.Endpoint);

        return false;
    }

    /// <inheritdoc />
    public bool Queue(byte[] buffer)
    {
        mBuffer = ByteBuffer.Wrap(buffer);
        return Request.Queue(mBuffer);
    }

    /// <inheritdoc />
    public bool Queue(byte[] buffer, int length)
    {
        mBuffer = ByteBuffer.Wrap(buffer);
        return Request.Queue(mBuffer, length);
    }

    /// <inheritdoc />
    public byte[] RequestWait(IUsbDevice device)
    {
        if (device is DroidUsbDevice usbDevice)
            usbDevice.DeviceConnection.RequestWait();

        return Buffer;
    }

    /// <inheritdoc />
    public async Task<byte[]> RequestWaitAsync(IUsbDevice device)
    {
        if (device is DroidUsbDevice usbDevice)
            await usbDevice.DeviceConnection.RequestWaitAsync();

        return Buffer;
    }

    /// <inheritdoc />
    public byte[] RequestWait(IUsbDevice device, int timeout)
    {
        if (device is DroidUsbDevice usbDevice)
            usbDevice.DeviceConnection.RequestWait(timeout);

        return Buffer;
    }

    /// <inheritdoc />
    public async Task<byte[]> RequestWaitAsync(IUsbDevice device, int timeout)
    {
        if (device is DroidUsbDevice usbDevice)
            await usbDevice.DeviceConnection.RequestWaitAsync(timeout);

        return Buffer;
    }
}