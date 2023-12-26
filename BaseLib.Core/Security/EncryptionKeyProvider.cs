using System.Security.Cryptography;

namespace BaseLib.Core.Security
{
    public class EncryptionKeyProvider : IEncryptionKeyProvider
    {
        private readonly byte[] masterKey;

        public EncryptionKeyProvider(byte[] masterKey)
        {
            this.masterKey = masterKey;
        }

        public Task<(byte[] key, byte[] wrappedKey)> GetEncryptionKeyAsync()
        {
            var dataKey = RandomNumberGenerator.GetBytes(32);
            var wrappedKey = WrapKey(dataKey);

            return Task.FromResult((dataKey, wrappedKey));
        }

        private byte[] WrapKey(byte[] dataKey)
        {
            return dataKey;
        }

        public Task<byte[]> UnwrapKeyAsync(byte[] wrappedKey)
        {
            var dataKey = UnwrapKey(wrappedKey);
            return Task.FromResult(dataKey);
        }

        private byte[] UnwrapKey(byte[] wrappedKey)
        {
            return wrappedKey;
        }
    }
}