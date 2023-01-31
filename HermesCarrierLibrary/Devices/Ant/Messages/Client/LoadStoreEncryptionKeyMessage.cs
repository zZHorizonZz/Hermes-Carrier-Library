using HermesCarrierLibrary.Devices.Ant.Enum;

namespace HermesCarrierLibrary.Devices.Ant.Messages.Client;

public class LoadStoreEncryptionKeyMessage : AntMessage
{
    public EncryptionOperation Operation { get; set; } = EncryptionOperation.Load;
    public byte KeyIndex { get; set; }
    public byte[] Key { get; set; }

    /// <inheritdoc />
    public LoadStoreEncryptionKeyMessage() : base(0x83, 18)
    {
    }

    /// <inheritdoc />
    public LoadStoreEncryptionKeyMessage(EncryptionOperation operation, byte keyIndex, byte[] key) : this()
    {
        Operation = operation;
        KeyIndex = keyIndex;
        Key = key;
    }

    /// <inheritdoc />
    public override void DecodePayload(BinaryReader payload)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override BinaryWriter EncodePayload()
    {
        switch (Operation)
        {
            case EncryptionOperation.Store when Key.Length != 16:
                throw new ArgumentException("Key must be 16 bytes long when storing a key.", nameof(Key));
            case EncryptionOperation.Load when Key.Length != 1:
                throw new ArgumentException("Invalid key index.", nameof(Key));
        }

        var payload = new BinaryWriter(new MemoryStream());
        payload.Write((byte)Operation);
        payload.Write(KeyIndex);
        payload.Write(Key);
        return payload;
    }
}