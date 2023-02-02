namespace HermesCarrierLibrary.Devices.Usb.Enum;

public enum UsbControlTransferType
{
    In = 0x80,
    Out = 0x00,
    DirectionMask = 0x80,
    NumberMask = 0x0F,
    XferBulk = 0x02,
    XferControl = 0x00,
    XferInterrupt = 0x03,
    XferIsoc = 0x01,
    XferMask = 0x03,
}