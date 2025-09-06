using BaseLib.Core.Models;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using BaseLib.Core.Serialization;

namespace BaseLib.Core.Services.AmazonCloud
{
    /// <summary>
    /// Sns Implementation of ICoreStatusEventSink, Writes serialized event to an AWS SNS Topic
    /// Use as a singleton is suggested.
    /// </summary>
    public class SnsCoreStatusEventSink : ICoreStatusEventSink
    {
        private readonly IAmazonSimpleNotificationService sns;
        private readonly string topicName;
        private Topic? topic;

        public SnsCoreStatusEventSink(IAmazonSimpleNotificationService sns, string topicName)
        {
            this.sns = sns;
            this.topicName = topicName;
        }

        public async Task WriteAsync(CoreStatusEvent statusEvent)
        {
            this.topic ??= await this.sns.FindTopicAsync(this.topicName);

            var message = CoreSerializer.Serialize(statusEvent);

            var response = statusEvent.Response;

            var request = new PublishRequest
            {
                MessageGroupId = statusEvent.CorrelationId ?? statusEvent.OperationId,
                MessageDeduplicationId = $"{statusEvent.OperationId}-{statusEvent.Status}",
                Message = message,
                TopicArn = topic.TopicArn,
                MessageAttributes = new Dictionary<string, MessageAttributeValue>
                {
                    { "ModuleName",  new MessageAttributeValue{ DataType = "String", StringValue = statusEvent.ModuleName } },
                    { "ServiceName",  new MessageAttributeValue{ DataType = "String", StringValue = statusEvent.ServiceName } },
                    { "Status",  new MessageAttributeValue{ DataType = "String", StringValue = statusEvent.Status.ToString() } },
                    { "Succeeded",  new MessageAttributeValue{ DataType = "String", StringValue = response != null ? response.Succeeded.ToString() : "False" } },
                    { "ReasonCode",  new MessageAttributeValue{ DataType = "String", StringValue = response != null ? response.ReasonCode.Value.ToString() : "0" } },
                    { "IsLongRunningService",  new MessageAttributeValue{ DataType = "String", StringValue = statusEvent.IsLongRunningService.ToString() } },
                    { "IsLongRunningChild",  new MessageAttributeValue{ DataType = "String", StringValue = statusEvent.IsLongRunningChild.ToString() } }
                }
            };

            await this.sns.PublishAsync(request);
        }
    }
}

