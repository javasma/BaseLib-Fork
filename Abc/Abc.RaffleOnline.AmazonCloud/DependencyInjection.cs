using System;
using Abc.RaffleOnline.AmazonCloud;
using Amazon.SQS;
using BaseLib.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Abc.RaffleOnline
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRaffleOnlineAmazonCloudServices(this IServiceCollection services)
        {

            services.AddSingleton<IAmazonSQS, AmazonSQSClient>();

            services.AddSingleton<ICoreServiceJournal, RaffleServiceJournalSqsProxy>();
            
            services.AddSingleton<ICoreServiceFireOnly>((sp)=>{
                var sqs = sp.GetRequiredService<IAmazonSQS>();
                return new SqsCoreServiceFireOnly(sqs, "raffle-service-queue");
            });

           
            return services;
        }
    }
}