using Amazon.S3.Model;

namespace Amazon.S3
{
    public static class AmazonS3Extensions
    {
        public static async Task<bool> ObjectExistsAsync(this IAmazonS3 s3, string bucketName, string key)
        {
            try
            {
                var metadata = await s3.GetObjectMetadataAsync(new GetObjectMetadataRequest
                {
                    BucketName = bucketName,
                    Key = key
                });

                //object exists
                return true;
            }
            catch (AmazonS3Exception ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    //object doesn't exists
                    return false;
                }
                //otherwise throw
                throw;
            }
        }

        /// <summary>
        /// returns object if exists, otherwise returns null
        /// </summary>
        public static async Task<(bool exists, GetObjectResponse? response)> GetObjectIfExistsAsync(this IAmazonS3 s3, GetObjectRequest request)
        {
            try
            {
                var response = await s3.GetObjectAsync(request);
                
                return (true, response);

            }
            catch (AmazonS3Exception ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    //object doesn't exists
                    return (false, null);
                }
                //otherwise throw
                throw;
            }

        }
    }
}