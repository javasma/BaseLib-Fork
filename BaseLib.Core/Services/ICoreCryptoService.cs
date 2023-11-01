namespace BaseLib.Core.Services
{
    public interface ICoreCryptoService
    {
        Task<byte[]> EncryptAsync(Stream plainStream, Stream encryptedStream);
        Task<Stream> DecryptAsync(Stream encryptedStream, byte[] encryptedDataKey);
    }
}