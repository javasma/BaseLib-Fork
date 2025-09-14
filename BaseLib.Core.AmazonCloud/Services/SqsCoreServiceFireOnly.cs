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
        private readonly SemaphoreSlim initLock = new(1, 1);
        private readonly SemaphoreSlim concurrencyLock;
        private readonly int batchSize;
        

        public SqsCoreServiceFireOnly(IAmazonSQS sqs, string queueName, int maxConcurrency = 10, int batchSize = 10)
        {
            this.sqs = sqs;
            this.queueName = queueName;
            this.batchSize = batchSize;
            this.concurrencyLock = new SemaphoreSlim(maxConcurrency, maxConcurrency);
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

            var messageBody = BuildMessageBody(typeName, request, correlationId, isLongRunningChild);

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

        private async Task InitializeAsync()
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

        public Task FireManyAsync<TService>(IEnumerable<CoreRequestBase> requests, string? correlationId = null, bool isLongRunningChild = false)
            where TService : ICoreServiceBase
        {
            var type = typeof(TService);

            var typeName = $"{type.FullName}, {type.Assembly.GetName().Name}"; //minimal, keep it simple

            return FireManyAsync(typeName, requests, correlationId, isLongRunningChild);

        }

        public async Task FireManyAsync(string typeName, IEnumerable<CoreRequestBase> requests, string? correlationId = null, bool isLongRunningChild = false)
        {
            await InitializeAsync();

            var batches = requests.Chunk(batchSize);
            var tasks = new List<Task>();

            foreach (var batch in batches)
            {
                await concurrencyLock.WaitAsync();

                tasks.Add(Task.Run(async () =>
                {
                    var entries = batch.Select(request =>
                    {
                        string messageBody = BuildMessageBody(typeName, request, correlationId, isLongRunningChild);
                        var messageGroupId = correlationId ?? Guid.NewGuid().ToString();
                        var dedupId = GetMessageDeduplicationId(messageBody);

                        return new SendMessageBatchRequestEntry
                        {
                            Id = Guid.NewGuid().ToString(),
                            MessageBody = messageBody,
                            MessageDeduplicationId = dedupId,
                            MessageGroupId = messageGroupId
                        };
                    }).ToList();

                    var batchRequest = new SendMessageBatchRequest
                    {
                        QueueUrl = queueUrl,
                        Entries = entries
                    };

                    // try send batch on finally release the lock
                    try
                    {
                        await sqs.SendMessageBatchAsync(batchRequest);
                    }
                    finally
                    {
                        concurrencyLock.Release();
                    }
                }));
            }

            await Task.WhenAll(tasks);
        }
        
        private static string BuildMessageBody(string typeName, CoreRequestBase request, string? correlationId, bool isLongRunningChild)
        {
            var message = new
            {
                TypeName = typeName,
                Method = "RunAsync",
                Request = request,
                CorrelationId = correlationId,
                IsLongRunningChild = isLongRunningChild
            };

            var messageBody = CoreSerializer.Serialize(message);
            return messageBody;
        }
    }
}