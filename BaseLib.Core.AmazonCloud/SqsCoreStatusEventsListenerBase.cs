using System.Text.Json;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using BaseLib.Core.Models;
using BaseLib.Core.Serialization;

namespace BaseLib.Core.AmazonCloud
{
    /// <summary>
    /// Base class for handling SQS events containing SNS notifications with CoreStatusEvent payloads.
    /// </summary>
    public abstract class SqsCoreStatusEventsListenerBase
    {
        public virtual async Task<SQSBatchResponse> HandleAsync(SQSEvent sqsEvent, ILambdaContext context)
        {
            var processingTasks = new Dictionary<string, Task>();

            foreach (var message in sqsEvent.Records)
            {

                processingTasks[message.MessageId] = HandleSingleMessageAsync(message);
            }

            try
            {
                await Task.WhenAll(processingTasks.Values);
            }
            catch
            {
                // Intentionally swallowing exceptions to allow batch failure handling below.
            }

            var batchItemFailures = processingTasks
                .Where(t => t.Value.IsFaulted)
                .Select(f => new SQSBatchResponse.BatchItemFailure { ItemIdentifier = f.Key })
                .ToList();

            return new SQSBatchResponse
            {
                BatchItemFailures = batchItemFailures
            };
        }

        private async Task HandleSingleMessageAsync(SQSEvent.SQSMessage message)
        {
            var snsNotification = JsonSerializer.Deserialize<SnsNotification>(message.Body);
            if (snsNotification?.Message == null)
                throw new NullReferenceException("No Message on SNS Notification");

            var coreEvent = CoreSerializer.Deserialize<CoreStatusEvent>(snsNotification.Message)
                ?? throw new NullReferenceException("No CoreStatusEvent on SNS Notification");

            await HandleStatusEventAsync(coreEvent);
        }

        protected abstract Task HandleStatusEventAsync(CoreStatusEvent coreEvent);

        private class SnsNotification
        {
            public string? Message { get; set; }
        }

    }
}
