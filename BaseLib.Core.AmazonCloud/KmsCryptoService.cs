using Amazon.KeyManagementService;
using Amazon.KeyManagementService.Model;
using System.Security.Cryptography;
using BaseLib.Core.Services;

namespace BaseLib.Core.AmazonCloud 
{
    public class KmsCryptoService : ICoreCryptoService
    {
        private readonly IAmazonKeyManagementService kmsClient;
        private readonly string kmsKeyName;

        public KmsCryptoService(IAmazonKeyManagementService kmsClient, string kmsKeyName)
        {
            this.kmsClient = kmsClient;
            this.kmsKeyName = kmsKeyName;
        }

        public async Task<byte[]> EncryptAsync(Stream plainStream, Stream encryptedStream)
        {
            // Step 1: Generate a data key with KMS
            var dataKeyResponse = await kmsClient.GenerateDataKeyAsync(new GenerateDataKeyRequest
            {
                KeyId = kmsKeyName,
                KeySpec = DataKeySpec.AES_256,
            });


            // Step 2: Encrypt the data using the generated data key
            using (var algorithm = GetSymmetricAlgorithm(dataKeyResponse.Plaintext.ToArray()))
            {
                using (var cryptoStream = new CryptoStream(encryptedStream, algorithm.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    plainStream.CopyTo(cryptoStream);
                }
                encryptedStream.Position = 0;
            }

            return dataKeyResponse.CiphertextBlob.ToArray();
        }

        public async Task<Stream> DecryptAsync(Stream encryptedStream, byte[] encryptedDataKey)
        {
            // Step 1: Decrypt the data key
            var decryptResponse = await kmsClient.DecryptAsync(new DecryptRequest
            {
                CiphertextBlob = new MemoryStream(encryptedDataKey),
            });

            // Step 2: Decrypt the data using the decrypted data key
            using var algorithm = GetSymmetricAlgorithm(decryptResponse.Plaintext.ToArray());
            var decryptedStream = new MemoryStream();
            using (var cryptoStream = new CryptoStream(encryptedStream, algorithm.CreateDecryptor(), CryptoStreamMode.Read))
            {
                cryptoStream.CopyTo(decryptedStream);
            }
            decryptedStream.Position = 0;
            return decryptedStream;
        }

        protected virtual SymmetricAlgorithm GetSymmetricAlgorithm(byte[] key)
        {
            SymmetricAlgorithm algorithm = Aes.Create();
            algorithm.Key = key;
            algorithm.Mode = CipherMode.CFB;
            algorithm.Padding = PaddingMode.PKCS7;
            return algorithm;

        }
    }
}