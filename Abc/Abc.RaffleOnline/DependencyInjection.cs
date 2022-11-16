using System;
using Abc.RaffleOnline.Raffles.OpenRaffle;
using Abc.RaffleOnline.Raffles.OpenRaffle.InProc;
using BaseLib.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Abc.RaffleOnline.Raffles.Billing;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using BaseLib.Core.AmazonCloud;

namespace Abc.RaffleOnline
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRaffleOnlineServices(this IServiceCollection services)
        {
            #region Transversal

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

            services.AddSingleton<IJournalEventHandler, RaffleJournalEventHandler>();



            #endregion
            #region Raffles

            services.AddTransient<IOpenRaffleService, OpenRaffleService>();
            services.AddSingleton<IRaffleWriter, RaffleInProcWriter>();
            services.AddTransient<IBillingService, BillingService>(); 

            #endregion

             //Fabrica de servicios a partir del typeName 
            services.AddTransient<Func<string, ICoreServiceBase>>((sp) => (typeName) =>
            {
                var type = Assembly.GetExecutingAssembly().GetType(typeName);
                return sp.GetService(type) as ICoreServiceBase;

            });

            services.AddSingleton<CoreServiceRunner>();



            return services;
        }
    }
}