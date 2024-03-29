﻿namespace HermesCarrierLibrary.Devices.Usb;

public class UsbControlTransfer
{
    public UsbControlTransfer(int requestType, int request, int value, int index, byte[] data,
        int? offset, int length, int timeout)
    {
        RequestType = requestType;
        Request = request;
        Value = value;
        Index = index;
        Data = data;
        Offset = offset;
        Length = length;
        Timeout = timeout;
    }

    public UsbControlTransfer(int requestType, int request, int value, int index, byte[] data,
        int length, int timeout)
    {
        RequestType = requestType;
        Request = request;
        Value = value;
        Index = index;
        Data = data;
        Length = length;
        Timeout = timeout;
    }

    public int RequestType { get; set; }
    public int Request { get; set; }
    public int Value { get; set; }

    public int Index { get; set; }
    public byte[] Data { get; set; }
    public int? Offset { get; set; }
    public int Length { get; set; }
    public int Timeout { get; set; }
}