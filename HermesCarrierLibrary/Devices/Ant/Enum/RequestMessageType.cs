﻿namespace HermesCarrierLibrary.Devices.Ant.Enum;

public enum RequestMessageType
{
    CHANNEL_STATUS = 0x52,
    CHANNEL_ID = 0x51,
    ANT_VERSION = 0x3E,
    CAPABILITIES = 0x54,
    SERIAL_NUMBER = 0x61,
    EVENT_BUFFERING_CONFIG = 0x74,
    ADVANCED_BURST_CAPABILITIES = 0x78,
    EVENT_FILTER = 0x79,
    SELECTIVE_DATA_UPDATE_MASK = 0x7B,
    USER_NVM = 0x7C,
    ENCRYPTION_MODE = 0x7D
}