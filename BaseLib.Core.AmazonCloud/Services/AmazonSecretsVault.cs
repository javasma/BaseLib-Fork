using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

namespace BaseLib.Core.Security.AmazonCloud
{
    public class AmazonSecretsVault : ICoreSecretsVault
    {
        private readonly IAmazonSecretsManager secretsManager;

        public AmazonSecretsVault(IAmazonSecretsManager amazonSecretsManager)
        {
            secretsManager = amazonSecretsManager;
        }

        public async Task<string> GetSecretValueAsync(string secretName)
        {

            var response = await secretsManager.GetSecretValueAsync(new GetSecretValueRequest
            {
                SecretId = secretName
            });

            return response.SecretString;

        }
    }

    
}