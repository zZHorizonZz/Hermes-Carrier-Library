using HermesCarrierLibrary.Devices.Ant.Enum;

namespace HermesCarrierLibrary.Devices.Ant.Messages.Device;

public class CapabilitiesMessage : AntMessage
{
    /// <inheritdoc />
    public CapabilitiesMessage() : base(0x54, 64)
    {
    }

    public byte MaxChannels { get; set; }
    public byte MaxNetworks { get; set; }
    public byte MaxSendRcoreChannels { get; set; }

    public Capabilities[] Capabilities { get; set; }
    public bool ReactiveNotifications { get; set; }

    /// <inheritdoc />
    public override void DecodePayload(BinaryReader payload)
    {
        MaxChannels = payload.ReadByte();
        MaxNetworks = payload.ReadByte();

        var firstOptionByte = payload.ReadByte();
        var secondOptionByte = payload.ReadByte();
        var thirdOptionByte = payload.ReadByte();

        MaxSendRcoreChannels = payload.ReadByte();

        var fourthOptionByte = payload.ReadByte();

        long optionBytes = firstOptionByte | (secondOptionByte << 8) | (thirdOptionByte << 16) |
                           (fourthOptionByte << 24);
        var capabilities = new List<Capabilities>();
        for (var i = 0; i < 32; i++)
            if ((optionBytes & (1 << i)) != 0)
                capabilities.Add((Capabilities)(optionBytes & (1 << i)));

        Capabilities = capabilities.ToArray();
        ReactiveNotifications = payload.ReadByte() != 0;
    }

    /// <inheritdoc />
    public override BinaryWriter EncodePayload()
    {
        throw new NotImplementedException();
    }
}