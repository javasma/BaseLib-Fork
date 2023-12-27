using Amazon.S3;
using Amazon.S3.Model;
using BaseLib.Core.Models;
using BaseLib.Core.Serialization;

namespace BaseLib.Core.Services.AmazonCloud
{
    /// <summary>
    /// Default S3 Implementation to store events
    /// </summary>
    public class CoreStatusEventsStore : ICoreStatusEventStore
    {
        private readonly IAmazonS3 s3;
        private readonly string bucketName;
        private readonly string folderName;

        public CoreStatusEventsStore(IAmazonS3 s3, string bucketName, string folderName = "events")
        {
            this.s3 = s3;
            this.bucketName = bucketName;
            this.folderName = folderName;
        }

        public Task<CoreStatusEvent> ReadAsync(string correlationId)
        {
            throw new NotImplementedException();
        }

        public async Task<int> WriteAsync(CoreStatusEvent statusEvent)
        {
            var keyName = $"{folderName}/{statusEvent.OperationId}.json";

            var s = CoreSerializer.Serialize(statusEvent);

            using (var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(s)))
            {
                var request = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName,
                    InputStream = stream,
                };

                await s3.PutObjectAsync(request);
            }

            return 1;
        }
    }
}