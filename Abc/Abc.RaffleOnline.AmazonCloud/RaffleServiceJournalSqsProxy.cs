using System.Threading.Tasks;
using Amazon.SQS;
using BaseLib.Core.Services;
using Newtonsoft.Json;

namespace Abc.RaffleOnline.AmazonCloud
{
    public class RaffleServiceJournalSqsProxy : ICoreServiceJournal
    {
        private const string queueName = "raffle-journal-queue";
        private readonly IAmazonSQS sqsClient;

        public RaffleServiceJournalSqsProxy(IAmazonSQS sqsClient)
        {
            this.sqsClient = sqsClient;
            
        }

        public async Task BeginAsync(ICoreServiceState state)
        {
            var r = await sqsClient.GetQueueUrlAsync(queueName);
            var queueUrl = r.QueueUrl;

            var message = new {
                Method = "BeginAsync",
                State = state as object
            };

            var messageBody = JsonConvert.SerializeObject(message, Formatting.Indented);

            await this.sqsClient.SendMessageAsync(queueUrl, messageBody);

        }

        public async Task EndAsync(ICoreServiceState state)
        {
            var r = await sqsClient.GetQueueUrlAsync(queueName);
            var queueUrl = r.QueueUrl;

            var message = new {
                Method = "EndAsync",
                State = state as object
            };

            var messageBody = JsonConvert.SerializeObject(message, Formatting.Indented);

            await this.sqsClient.SendMessageAsync(queueUrl, messageBody);
        }
    }
}