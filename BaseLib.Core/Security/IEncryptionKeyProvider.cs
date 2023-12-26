namespace BaseLib.Core.Security
{
    public interface IEncryptionKeyProvider
    {
        Task<(byte[] key, byte[] wrappedKey)> GetEncryptionKeyAsync();
        Task<byte[]> UnwrapKeyAsync(byte[] wrappedKey);
    }
}