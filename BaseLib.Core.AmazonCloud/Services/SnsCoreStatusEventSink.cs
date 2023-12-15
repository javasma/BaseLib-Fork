using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Newtonsoft.Json;

namespace BaseLib.Core.Services.AmazonCloud
{
    public class SnsCoreStatusEventSink : ICoreStatusEventSink
    {
        private readonly IAmazonSimpleNotificationService sns;
        private readonly string topicName;

        public SnsCoreStatusEventSink(IAmazonSimpleNotificationService sns, string topicName)
        {
            this.sns = sns;
            this.topicName = topicName;
        }

        public async Task WriteAsync(ICoreStatusEvent statusEvent)
        {
            try
            {             
                var topic = await this.sns.FindTopicAsync(this.topicName);
             
                var message = JsonConvert.SerializeObject(statusEvent);

                var response = statusEvent.Response as ICoreServiceResponse;

                var request = new PublishRequest
                {
                    MessageGroupId = statusEvent.OperationId,
                    MessageDeduplicationId = $"{statusEvent.OperationId}-{statusEvent.Status}",
                    Message = message,
                    TopicArn = topic.TopicArn,
                    MessageAttributes = new Dictionary<string, MessageAttributeValue>
                    {
                        { "ServiceName",  new MessageAttributeValue{ DataType = "String", StringValue = statusEvent.ServiceName } },
                        { "Status",  new MessageAttributeValue{ DataType = "String", StringValue = statusEvent.Status.ToString() } },
                        { "Succeeded",  new MessageAttributeValue{ DataType = "String", StringValue = response !=null ? response.Succeeded.ToString() : "False" } },
                        { "ReasonCode",  new MessageAttributeValue{ DataType = "String", StringValue = response!=null ? Convert.ToInt32(response.ReasonCode).ToString() : "0" } }
                    }
                };

                await this.sns.PublishAsync(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message},\n{ex.StackTrace}");
                throw;
            }
        }
    }
}
