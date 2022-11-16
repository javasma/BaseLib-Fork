using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using BaseLib.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace Abc.RaffleOnline.Serverless
{

    /// <summary>
    /// Procesador de los eventos del modulo, 
    /// pegado a una cola que recibe todos los eventos e invoca el journal 
    /// </summary>
    public class JournalEventsProcessor
    {
        public JournalEventsProcessor()
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
            context.Logger.Log($"Processing message {message.Body}");

            using (var scope = ServiceProviderHelper.CreateScope())
            {
                var handler = scope.ServiceProvider.GetRequiredService<IJournalEventHandler>();
                var payload = JsonConvert.DeserializeObject<Payload>(message.Body) ;
                var statusEvent = JsonConvert.DeserializeObject<CoreStatusEvent>(payload.Message);

                if (statusEvent != null)
                {
                    context.Logger.Log($"status event is: {JsonConvert.SerializeObject(statusEvent, Formatting.Indented)}");
                    await handler.HandleAsync(statusEvent);
                }
                else
                {
                    context.Logger.Log($"Unable to deserialize status event {message.MessageId}");
                }
            }
        }

        private class Payload
        {
            public string Message{get;set;}
        }

    }
}