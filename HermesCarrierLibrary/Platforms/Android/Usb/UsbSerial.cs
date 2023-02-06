namespace HermesCarrierLibrary.Platforms.Android.Devices;

public class UsbDeviceImpl //: ISerial, IUsbDevice
{
    /* public const int TIMEOUT = 5000;
 
     private readonly WeakEventManager mCloseEventManager = new();
 
     private readonly WeakEventManager mOpenEventManager = new();
 
     private readonly object mReadLock = new();
     private readonly object mWriteLock = new();
 
     private DroidUsbDevice mUsbDevice;
     private UsbDeviceConnection mUsbDeviceConnection;
 
     private DroidUsbEndpoint mUsbEndpointIn;
     private DroidUsbEndpoint mUsbEndpointOut;
     private DroidUsbInterface mUsbInterface;
 
     private UsbManager mUsbManager;
 
     private DroidUsbRequest mUsbRequestIn;
 
     public UsbDeviceImpl(DroidUsbDevice device)
     {
         Name = device.ProductName ?? device.DeviceName;
 
         ProductId = device.ProductId;
         VendorId = device.VendorId;
     }
 
     /// <inheritdoc />
     public bool IsConnected { get; private set; }
 
     /// <inheritdoc />
     public string Name { get; }
 
     public event EventHandler Opened
     {
         add => mOpenEventManager.AddEventHandler(value);
         remove => mOpenEventManager.RemoveEventHandler(value);
     }
 
     public event EventHandler Closed
     {
         add => mCloseEventManager.AddEventHandler(value);
         remove => mCloseEventManager.RemoveEventHandler(value);
     }
 
     /// <inheritdoc />
     public void Write(byte[] data)
     {
         if (!IsConnected) throw new Exception("Device not connected");
 
         lock (mWriteLock)
         {
             mUsbDeviceConnection.BulkTransfer(mUsbEndpointOut, data, data.Length, TIMEOUT);
         }
     }
 
     /// <inheritdoc />
     public byte[] Read()
     {
         if (!IsConnected) throw new Exception("Device not connected");
 
         var buffer = new byte[mUsbEndpointIn.MaxPacketSize];
         lock (mReadLock)
         {
             var buf = ByteBuffer.Wrap(buffer);
             if (!mUsbRequestIn.Queue(buf, buffer.Length)) throw new IOException("Queueing USB request failed");
 
             var response = mUsbDeviceConnection.RequestWait();
             if (response == null) throw new IOException("Waiting for USB request failed");
 
             Buffer.BlockCopy(buf.ToByteArray(), 0, buffer, 0, buffer.Length);
             var read = buf.Position();
             return buffer[..read];
         }
     }
 
     /// <inheritdoc />
     public byte[] Read(int length)
     {
         if (!IsConnected) throw new Exception("Device not connected");
 
         var buffer = new byte[length];
         lock (mReadLock)
         {
             var buf = ByteBuffer.Wrap(buffer);
             if (!mUsbRequestIn.Queue(buf, buffer.Length)) throw new IOException("Queueing USB request failed");
 
             var response = mUsbDeviceConnection.RequestWait();
             if (response == null) throw new IOException("Waiting for USB request failed");
 
             Buffer.BlockCopy(buf.ToByteArray(), 0, buffer, 0, buffer.Length);
             var read = buf.Position();
             return buffer[..read];
         }
     }
 
     /// <inheritdoc />
     public int VendorId { get; }
 
     /// <inheritdoc />
     public int ProductId { get; }
 
     public void Close()
     {
         if (!IsConnected) throw new Exception("Device not connected");
 
         mUsbDeviceConnection.ReleaseInterface(mUsbInterface);
         mUsbDeviceConnection.Close();
         IsConnected = false;
 
         mCloseEventManager.HandleEvent(this, EventArgs.Empty, nameof(Closed));
     }
 
     /// <inheritdoc />
     public void SetBaudRate(int baudRate)
     {
         if (!IsConnected) throw new Exception("Device not connected");
 
         mUsbDevice.DeviceClass.
     }
 
     public void Open(Context? context)
     {
         mUsbManager = (UsbManager)context?.GetSystemService(Context.UsbService);
         mUsbDevice =
             mUsbManager?.DeviceList?.Values.FirstOrDefault(d => d.ProductId == ProductId && d.VendorId == VendorId);
 
         if (mUsbDevice == null)
             throw new Exception("Device not found");
 
         mUsbInterface = mUsbDevice.GetInterface(0);
         for (var i = 0; i < mUsbInterface.EndpointCount; i++)
         {
             var endpoint = mUsbInterface.GetEndpoint(i);
             if (endpoint is not { Type: UsbAddressing.XferBulk }) continue;
 
             if (endpoint.Direction == UsbAddressing.In)
                 mUsbEndpointIn = endpoint;
             else
                 mUsbEndpointOut = endpoint;
         }
 
         mUsbDeviceConnection = mUsbManager.OpenDevice(mUsbDevice);
 
         if (mUsbDeviceConnection == null)
             throw new Exception("Connection not found");
 
         mUsbDeviceConnection.ClaimInterface(mUsbInterface, true);
         Console.WriteLine("Opened connection to ANT+ dongle "
                           + Name + " EndpointIn: " + mUsbEndpointIn +
                           " EndpointOut: " + mUsbEndpointOut);
 
         mUsbRequestIn = new DroidUsbRequest();
         mUsbRequestIn.Initialize(mUsbDeviceConnection, mUsbEndpointIn);
         IsConnected = true;
 
         mOpenEventManager.HandleEvent(this, EventArgs.Empty, nameof(Opened));
     }*/
}