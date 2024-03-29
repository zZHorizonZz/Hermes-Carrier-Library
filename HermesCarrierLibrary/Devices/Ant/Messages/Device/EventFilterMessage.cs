﻿namespace HermesCarrierLibrary.Devices.Ant.Messages.Device;

public class EventFilterMessage : AntMessage
{
    /// <inheritdoc />
    public EventFilterMessage() : base(0x79, 3)
    {
    }

    public ushort EventFilter { get; set; }

    /// <inheritdoc />
    public override void DecodePayload(BinaryReader payload)
    {
        ChannelNumber = payload.ReadByte();
        EventFilter = payload.ReadUInt16();
    }

    /// <inheritdoc />
    public override BinaryWriter EncodePayload()
    {
        throw new NotImplementedException();
    }
}