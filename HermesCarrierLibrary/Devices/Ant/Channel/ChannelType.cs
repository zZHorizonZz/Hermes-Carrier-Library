namespace HermesCarrierLibrary.Devices.Ant.Channel;

public enum ChannelType
{
    ReceiveChannel = 0x00,
    TransmitChannel = 0x10,
    TransmitOnlyChannel = 0x50,
    ReceiveOnlyChannel = 0x40,
    SharedBidirectionalReceiveChannel = 0x20,
    SharedBidirectionalTransmitChannel = 0x30
}