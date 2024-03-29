<p align="center">
	<a href="https://www.nuget.org/packages/HermesCarrierLibrary/"><img src="https://img.shields.io/nuget/v/HermesCarrierLibrary?style=for-the-badge" alt="NuGet"></a>
  <a href="https://www.nuget.org/packages/HermesCarrierLibrary/"><img src="https://img.shields.io/nuget/dt/HermesCarrierLibrary?style=for-the-badge" alt="NuGet Downloads"></a>
</p>

# Hermes Carrier Library

> Hermes Library is a set of APIs that allow developers to access and interact with ant, usb and bluetooth devices
> within
> their applications. The library provides an easy-to-use interface for managing communication between devices and the
> application.

## Features

- **Ant**: The Ant library provides access to ant devices trough **Ant Dongle** and allows you to send and receive data
  from these devices. You can also set up and manage ant channels, broadcast data, and receive responses from the
  devices. For more information about ant, you can visit
  the [ant developer website](https://www.thisisant.com/developer/resources/downloads/#software_tab).

- **USB**: The USB library provides access to USB devices and allows you to send and receive data from these devices.
  You can also configure the communication between the application and the device, including setting up transfer and
  control transfers. The library is heavily based on
  android's [USB host API](https://developer.android.com/guide/topics/connectivity/usb/host)

- **Bluetooth**: ~~The Bluetooth library provides access to Bluetooth devices and allows you to send and receive data
  from
  these devices. You can also manage the pairing of devices and configure the communication settings between the
  application and the device.~~ (Not yet implemented)

## Getting Started

> The library is available as a NuGet package, and you can install it using the following command in the Package Manager
> Console:

```bash
Install-Package HermesCarrierLibrary
```

## Usage

### Ant

> In this example we will show how to use the Ant library to send a ResetSystemMessage to an Ant Dongle.

```csharp
// We can get a list of all the Ant Dongles connected to the device or we can detect newly connected devices
// We will use the first one
var trasmitter = deviceService.AntTransmitters.FirstOrDefault();

// We can now open the Ant Dongle
if(!transmitter.IsConnected)
    await transmitter.OpenAsync();

// If everything went well, we can now start using the Ant Dongle        
await transmitter.SendMessageAsync(new ResetSystemMessage()); // Will send the ResetSystemMessage to the Ant Dongle
```

### USB

> In this example we will show how to use the USB library to open a USB device and send a message to it.

```csharp
// Now we can get list of all the USB devices connected to the device
var devices = deviceService.UsbDevices.FirstOrDefault();

// We can get usb interfaces from the device
var interfaces = device.Interfaces.First();

// We can also get the endpoints from the interface
var endpoint = interfaces.Endpoints.First();

// We can now open the USB device
await device.OpenAsync();

// If everything went well, we can now start using the USB device
await device.ClaimInterfaceAsync(interface); // Claim the interface

// Now we can send the message
var bytes = new byte[0];
var transfer = new UsbBulkTransfer(endpoint, bytes, bytes.Length, 1000);
var result = await device.BulkTransfer(transfer);
```

## Bluetooth

> The Bluetooth library is not yet implemented.

## Demo App

> The demo app is a simple application that allows you to test the library and see how it works. You can find the demo
> app in the [Hermes Demo App](https://github.com/zZHorizonZz/Hermes-Carrier-Library/tree/master/HermesCarrierDemo)

## OS Support

| OS      | Ant | USB | Bluetooth |
|---------|-----|-----|-----------|
| Android | ✅   | ✅   | ⌛         |
| iOS     | ❌   | ❌   | ⌛         |
| Windows | ⌛   | ⌛   | ⌛         |

- ✅: Supported
- ❌: Not supported
- ⌛: Planned

## License

> Hermes Library is licensed under
> the [Apache License, Version 2.0](https://github.com/zZHorizonZz/Hermes-Carrier-Library/blob/master/LICENSE)

## Contributing

> Contributions are welcome! If you want to contribute or just have a idea to the project, please open an issue or a
> pull request.
