using System.Security.Cryptography;
using System.Text;
using Amazon.SQS;
using Amazon.SQS.Model;
using BaseLib.Core.Models;
using BaseLib.Core.Serialization;

namespace BaseLib.Core.Services.AmazonCloud
{
    public class SqsCoreServiceFireOnly : ICoreServiceFireOnly
    {
        private readonly IAmazonSQS sqs;
        private readonly string queueName;

        public SqsCoreServiceFireOnly(IAmazonSQS sqs, string queueName)
        {
            this.sqs = sqs;
            this.queueName = queueName;
        }

        public async Task FireAsync<TService>(CoreRequestBase request, string? correlationId = null)
            where TService : ICoreServiceBase
        {
            var r = await sqs.GetQueueUrlAsync(this.queueName);
            var queueUrl = r.QueueUrl;

            var message = new
            {
                Service = typeof(TService).FullName,
                Request = request,
                CorrelationId = correlationId
            };

            var messageBody = CoreSerializer.Serialize(message);

            var messageRequest = new SendMessageRequest
            {
                MessageBody = messageBody,
                MessageDeduplicationId = GetMessageDeduplicationId(messageBody),
                MessageGroupId = correlationId ?? Guid.NewGuid().ToString(),
                QueueUrl = queueUrl
            };

            await this.sqs.SendMessageAsync(messageRequest);

        }

        private string GetMessageDeduplicationId(string messageBody)
        {
            byte[] hashBytes = MD5.HashData(Encoding.UTF8.GetBytes(messageBody));
            return BitConverter.ToString(hashBytes); 
        }
    }
}