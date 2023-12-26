using Amazon.KeyManagementService;
using Amazon.KeyManagementService.Model;

namespace BaseLib.Core.Security.AmazonCloud
{
    public class KmsEncryptionKeyProvider : IEncryptionKeyProvider
    {
        private readonly IAmazonKeyManagementService kmsClient;
        private readonly string kmsKeyName;

        public KmsEncryptionKeyProvider(IAmazonKeyManagementService kmsClient, string kmsKeyName)
        {
            this.kmsClient = kmsClient;
            this.kmsKeyName = kmsKeyName;
        }

        /// <summary>
        /// Generate the encryption key with KMS GenerateDataKey method
        /// </summary>
        public async Task<(byte[] key, byte[] wrappedKey)> GetEncryptionKeyAsync()
        {
            var response = await kmsClient.GenerateDataKeyAsync(new GenerateDataKeyRequest
            {
                KeyId = kmsKeyName,
                KeySpec = DataKeySpec.AES_256,

            });
            return (response.Plaintext.ToArray(), response.CiphertextBlob.ToArray());
        }


        public async Task<byte[]> UnwrapKeyAsync(byte[] wrappedKey)
        {
            var response = await kmsClient.DecryptAsync(new DecryptRequest
            {
                CiphertextBlob = new MemoryStream(wrappedKey)
            });
            return response.Plaintext.ToArray();
        }
    }
}