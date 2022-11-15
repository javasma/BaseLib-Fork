using Amazon.SQS;
using BaseLib.Core.AmazonCloud;
using BaseLib.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Amazon.SimpleNotificationService;

namespace Abc.RaffleOnline.Serverless
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            //add default services from RaffleOnline dll
            services.AddRaffleOnlineServices();

            //add aws dependencies
            services.AddSingleton<IAmazonSQS, AmazonSQSClient>();
            services.AddSingleton<IAmazonSimpleNotificationService, AmazonSimpleNotificationServiceClient>();

            //the fire n forget invoker
            services.AddSingleton<ICoreServiceFireOnly>(sp =>
            {
                var sqs = sp.GetRequiredService<IAmazonSQS>();
                return new SqsCoreServiceFireOnly(sqs, "raffle-service-queue");
            });

            //the event sink service
            services.AddSingleton<ICoreStatusEventSink>(sp =>
            {
                var sns = sp.GetRequiredService<IAmazonSimpleNotificationService>();
                return new SnsCoreStatusEventSink(sns, "raffle-core-events-topic.fifo");
            });


            return services;
        }
    }
}