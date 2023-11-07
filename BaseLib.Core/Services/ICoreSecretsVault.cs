namespace BaseLib.Core.Services
{
    public interface ICoreSecretsVault
    {
        Task<string> GetSecretValueAsync(string secretName);
    }
}