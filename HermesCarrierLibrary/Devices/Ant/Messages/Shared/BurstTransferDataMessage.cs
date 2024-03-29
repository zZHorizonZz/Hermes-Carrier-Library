﻿namespace HermesCarrierLibrary.Devices.Ant.Messages.Shared;

public class BurstTransferDataMessage : AntMessage
{
    /// <inheritdoc />
    public BurstTransferDataMessage() : base(0x50, 15)
    {
    }

    /// <inheritdoc />
    public BurstTransferDataMessage(byte sequenceNumber, byte[] data) : this()
    {
        SequenceNumber = sequenceNumber;
        Data = data;
    }

    /// <inheritdoc />
    public BurstTransferDataMessage(byte sequenceNumber,
        byte[] data,
        bool extended,
        ushort deviceNumber,
        byte deviceType,
        byte transmissionType) : this()
    {
        SequenceNumber = sequenceNumber;
        Data = data;
        Extended = extended;
        DeviceNumber = deviceNumber;
        DeviceType = deviceType;
        TransmissionType = transmissionType;
    }

    public byte SequenceNumber { get; set; }
    public byte[] Data { get; set; }
    public bool Extended { get; set; }
    public ushort DeviceNumber { get; set; }
    public byte DeviceType { get; set; }
    public byte TransmissionType { get; set; }

    /// <inheritdoc />
    public override void DecodePayload(BinaryReader payload)
    {
        var flags = payload.ReadByte();
        SequenceNumber = (byte)(flags >> 5);
        ChannelNumber = (byte)(flags & 0x1F);
        
        Data = payload.ReadBytes(8);

        if (payload.BaseStream.Position == Length) return;

        Extended = payload.ReadBoolean();
        DeviceNumber = payload.ReadUInt16();
        DeviceType = payload.ReadByte();
        TransmissionType = payload.ReadByte();
    }

    /// <inheritdoc />
    public override BinaryWriter EncodePayload()
    {
        var payload = new BinaryWriter(new MemoryStream());
        var flags = (byte)0;
        flags |= (byte)(SequenceNumber << 5);
        flags |= ChannelNumber;
        
        payload.Write(flags);
        payload.Write(Data);

        if (!Extended) return payload;

        payload.Write(Extended);
        payload.Write(DeviceNumber);
        payload.Write(DeviceType);
        payload.Write(TransmissionType);

        return payload;
    }
}