using System.Net;
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
        private string? queueUrl;
        private bool isInitialized = false;
        private readonly SemaphoreSlim initLock = new SemaphoreSlim(1, 1);

        public SqsCoreServiceFireOnly(IAmazonSQS sqs, string queueName)
        {
            this.sqs = sqs;
            this.queueName = queueName;
        }

        public Task FireAsync<TService>(CoreRequestBase request, string? correlationId = null, bool isLongRunningChild = false)
            where TService : ICoreServiceBase
        {
            var type = typeof(TService);
            var typeName = $"{type.FullName}, {type.Assembly.GetName().Name}"; //minimal, keep it simple

            return FireAsync(typeName, request, correlationId, isLongRunningChild);
        }

        public async Task FireAsync(string typeName, CoreRequestBase request, string? correlationId = null, bool isLongRunningChild = false)
        {
            await InitializeAsync();

            var message = new
            {
                TypeName = typeName,
                Method = "RunAsync",
                Request = request,
                CorrelationId = correlationId,
                IsLongRunningChild = isLongRunningChild
            };

            var messageBody = CoreSerializer.Serialize(message);

            var messageRequest = new SendMessageRequest
            {
                MessageBody = messageBody,
                MessageDeduplicationId = GetMessageDeduplicationId(messageBody),
                MessageGroupId = correlationId ?? Guid.NewGuid().ToString(),
                QueueUrl = queueUrl
            };

            var response = await this.sqs.SendMessageAsync(messageRequest);

            //throw if not successful response
            if (response.HttpStatusCode != HttpStatusCode.OK)
            {
                throw new InvalidOperationException("Failed to send RunAsync message to SQS");
            }
        }

        public Task ResumeAsync<TService>(string operationId, string? correlationId = null)
            where TService : ICoreLongRunningService
        {
            var type = typeof(TService);

            var typeName = $"{type.FullName}, {type.Assembly.GetName().Name}"; //minimal, keep it simple

            return ResumeAsync(typeName, operationId, correlationId);
        }

        public async Task ResumeAsync(string typeName, string operationId, string? correlationId = null)
        {
            await InitializeAsync();

            var message = new
            {
                TypeName = typeName,
                Method = "ResumeAsync",
                OperationId = operationId,
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

            var response = await this.sqs.SendMessageAsync(messageRequest);

            //throw if not successful response
            if (response.HttpStatusCode != HttpStatusCode.OK)
            {
                throw new InvalidOperationException("Failed to send ResumeAsync message to SQS");
            }
        }

        public async Task InitializeAsync()
        {
            if (!isInitialized)
            {
                await initLock.WaitAsync();
                try
                {
                    if (!isInitialized)
                    {
                        var response = await sqs.GetQueueUrlAsync(queueName);
                        queueUrl = response.QueueUrl;
                        isInitialized = true;
                    }
                }
                finally
                {
                    initLock.Release();
                }
            }
        }
        private string GetMessageDeduplicationId(string messageBody)
        {
            byte[] hashBytes = MD5.HashData(Encoding.UTF8.GetBytes(messageBody));
            return BitConverter.ToString(hashBytes);
        }
    }
}