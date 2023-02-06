# Hermes Library

Hermes Library is a set of APIs that allow developers to access and interact with ant, usb and bluetooth devices within
their applications. The library provides an easy-to-use interface for managing communication between devices and the
application.

## Features

- **Ant**: The Ant library provides access to ant devices trough **Ant Dongle** and allows you to send and receive data
  from these devices. You can also set up and manage ant channels, broadcast data, and receive responses from the
  devices.

- **USB**: The USB library provides access to USB devices and allows you to send and receive data from these devices.
  You can also configure the communication between the application and the device, including setting up transfer and
  control transfers.

- **Bluetooth**: ~~The Bluetooth library provides access to Bluetooth devices and allows you to send and receive data
  from
  these devices. You can also manage the pairing of devices and configure the communication settings between the
  application and the device.~~ (Not yet implemented)

## Getting Started

The library is available as a NuGet package, and you can install it using the following command in the Package Manager
Console:

```bash
Install-Package HermesLibrary
```

## Usage

### Ant

In this example we will show how to use the Ant library to send a ResetSystemMessage to an Ant Dongle.

```csharp
// We need a DeviceService to access the Ant Dongle
var deviceService = new DeviceService();

// Now we can access the Ant Service
var antService = deviceService.AntService;

// We can get a list of all the Ant Dongles connected to the device or we can detect newly connected devices
// We will use the first one
var trasmitter = antService.DetectTransmitters().FirstOrDefault();

// We can now open the Ant Dongle
if(!transmitter.IsConnected)
    await transmitter.OpenAsync();

// If everything went well, we can now start using the Ant Dongle        
await transmitter.SendMessageAsync(new ResetSystemMessage()); // Will send the ResetSystemMessage to the Ant Dongle
```

### USB

In this example we will show how to use the USB library to open a USB device and send a message to it.

```csharp
// TODO
```
