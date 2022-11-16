﻿using System.Threading.Tasks;
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

            var request = new PublishRequest{
                MessageGroupId = statusEvent.OperationId,
                MessageDeduplicationId = $"{statusEvent.OperationId}-{statusEvent.Status}",
                Message = message,
                TopicArn = topic.TopicArn
            };

            await this.sns.PublishAsync(request);
            

        }
    }
}