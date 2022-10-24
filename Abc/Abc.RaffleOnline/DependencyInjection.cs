using Abc.RaffleOnline.Raffles.OpenRaffle;
using Abc.RaffleOnline.Raffles.OpenRaffle.InProc;
using BaseLib.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Abc.RaffleOnline
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRaffleOnlineServices(this IServiceCollection services)
        {
            services.AddSingleton<ICoreServiceJournal, RaffleServiceJournal>();
            
            #region Raffles

            services.AddTransient<OpenRaffleService>();
            services.AddSingleton<IRaffleWriter, RaffleInProcWriter>();
            

            #endregion

            return services;
        }
    }
}