using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using BaseLib.Core.Services;
using Newtonsoft.Json;

namespace BaseLib.Core.AmazonCloud
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
            var topic = await this.sns.FindTopicAsync(this.topicName);

            var message = JsonConvert.SerializeObject(statusEvent);

            var response = statusEvent.Response as ICoreServiceResponse;

            var request = new PublishRequest{
                MessageGroupId = statusEvent.OperationId,
                MessageDeduplicationId = $"{statusEvent.OperationId}-{statusEvent.Status}",
                Message = message,
                TopicArn = topic.TopicArn,
                MessageAttributes = new Dictionary<string, MessageAttributeValue>{
                    { "ServiceName",  new MessageAttributeValue{ DataType = "String", StringValue = statusEvent.ServiceName } },
                    { "Status",  new MessageAttributeValue{ DataType = "String", StringValue = statusEvent.Status.ToString() } },
                    { "Succeeded",  new MessageAttributeValue{ DataType = "String", StringValue = response?.Succeeded.ToString() } },
                    { "ReasonCode",  new MessageAttributeValue{ DataType = "String", StringValue = Convert.ToInt32(response?.ReasonCode).ToString() } }
                }
            };

            await this.sns.PublishAsync(request);
        }
    }
}
