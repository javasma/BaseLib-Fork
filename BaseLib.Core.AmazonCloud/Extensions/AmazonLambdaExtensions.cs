using System.IO.Compression;

namespace Amazon.Lambda.ApplicationLoadBalancerEvents
{
    public static class AmazonLambdaExtensions
    {
        public static ApplicationLoadBalancerResponse ToGZip(this ApplicationLoadBalancerResponse response)
        {
            var headers = response.Headers ?? new Dictionary<string, string>();
            headers["Content-Encoding"] = "gzip";

            string body = string.Empty;
            if (!string.IsNullOrEmpty(response.Body))
            {
                using (var compressedStream = new MemoryStream())
                using (var gzipStream = new GZipStream(compressedStream, CompressionMode.Compress))
                using (var writer = new StreamWriter(gzipStream))
                {
                    writer.Write(response.Body);
                    writer.FlushAsync();
                    compressedStream.Position = 0;
                    body = Convert.ToBase64String(compressedStream.ToArray());
                }
            }
            return new ApplicationLoadBalancerResponse
            {
                StatusCode = response.StatusCode,
                Headers = headers,
                Body = body,
                IsBase64Encoded = true
            };
        }
    }



}