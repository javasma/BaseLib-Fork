namespace BaseLib.Core.Security
{
    public interface ICoreSecretsVault
    {
        Task<string> GetSecretValueAsync(string secretName);
    }
}