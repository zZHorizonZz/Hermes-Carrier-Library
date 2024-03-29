﻿using HermesCarrierLibrary.Devices.Ant.Enum;

namespace HermesCarrierLibrary.Devices.Ant.Messages.Device;

public class EventResponseMessage : AntMessage
{
    /// <inheritdoc />
    public EventResponseMessage() : base(0x40, 3)
    {
    }

    public byte OriginalMessage { get; private set; }
    public EventResponseType Type { get; private set; }

    /// <inheritdoc />
    public override void DecodePayload(BinaryReader payload)
    {
        ChannelNumber = payload.ReadByte();
        OriginalMessage = payload.ReadByte();
        Type = EventResponseType.UNKNOWN_RESPONSE_CODE;

        var type = (int)payload.ReadByte();
        if (System.Enum.IsDefined(typeof(EventResponseType), type)) Type = (EventResponseType)type;
    }

    /// <inheritdoc />
    public override BinaryWriter EncodePayload()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"EventResponseMessage: ChannelNumber: {ChannelNumber}," +
               $" OriginalMessage: {OriginalMessage:X2}," +
               $" Type: {Type}";
    }
}