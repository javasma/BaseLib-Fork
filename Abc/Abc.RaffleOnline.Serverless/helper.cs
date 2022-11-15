
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Abc.RaffleOnline.Serverless
{
    public static class ServiceProviderHelper
    {
        private static readonly object syncObject = new object();
        private static IServiceProvider serviceProvider;

        private static IServiceProvider CreateServiceProvider()
        {
            if (serviceProvider == null)
            {
                lock (syncObject)
                {
                    if (serviceProvider == null)
                    {
                        var configuration = new ConfigurationBuilder() // ConfigurationBuilder() method requires Microsoft.Extensions.Configuration NuGet package
                            .AddEnvironmentVariables() // AddEnvironmentVariables() method requires Microsoft.Extensions.Configuration.EnvironmentVariables NuGet package
                            .Build();
                        var services = new ServiceCollection()
                            .AddCoreServices();
                        serviceProvider = services.BuildServiceProvider();

                    }
                }
            }
            return serviceProvider;
        }

        public static IServiceScope CreateScope()
        {
            var serviceProvider = CreateServiceProvider();
            return serviceProvider.CreateScope();
        }
    }
}
