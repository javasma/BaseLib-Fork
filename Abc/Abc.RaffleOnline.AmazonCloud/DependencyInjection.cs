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

            services.AddSingleton<IAmazonSQS>((sp) =>
            {
                return new AmazonSQSClient();
            });

            services.AddSingleton<ICoreServiceJournal, RaffleServiceJournalSqsProxy>();
            
            return services;
        }
    }
}