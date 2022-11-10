using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using BaseLib.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Abc.RaffleOnline.Serverless
{

    public class ServiceMessageProcessor
    {
        public ServiceMessageProcessor()
        {

        }

        public async Task SqsEventHandler(SQSEvent sqsEvent, ILambdaContext context)
        {
            foreach (var message in sqsEvent.Records)
            {
                await ProcessMessageAsync(message, context);
            }
        }

        private async Task ProcessMessageAsync(SQSEvent.SQSMessage message, ILambdaContext context)
        {
            context.Logger.Log($"Processing system message {message.Body}");
            
            using( var scope = ServiceProviderHelper.CreateScope())
            {
                var runner = scope.ServiceProvider.GetRequiredService<CoreServiceRunner>();
                await runner.RunAsync(message.Body);

            }
        }
    }
}