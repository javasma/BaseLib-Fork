namespace Amazon.Runtime.CredentialManagement
{
    public class AwsCredentialsFactory
    {
        public static AWSCredentials Create()
        {
            var profileName = Environment.GetEnvironmentVariable("AWS_PROFILE");

            if (profileName == null) throw new NullReferenceException(nameof(profileName));

            var chain = new CredentialProfileStoreChain();

            chain.TryGetAWSCredentials(profileName, out AWSCredentials credentials);

            return credentials;
        }
    }
} 