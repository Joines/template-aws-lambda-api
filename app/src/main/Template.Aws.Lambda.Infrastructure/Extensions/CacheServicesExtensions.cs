using Template.Aws.Lambda.Infrastructure.Configurations;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;


namespace Template.Aws.Lambda.Infrastructure.Extensions
{
    public static class CacheServicesExtensions
    {
        public static IServiceCollection AddCache(
            this IServiceCollection services, 
            AppConfigurations appConfigs)
        {
            services.AddMemoryCache();

            if (Debugger.IsAttached)
                services.AddDistributedMemoryCache();
            else
            {
                var appInfo = appConfigs.Splunk?.ApplicationInfo;

                services.AddStackExchangeRedisCache(options =>
                {
                    options.ConfigurationOptions = new ConfigurationOptions()
                    {
                        Ssl = true,
                        AbortOnConnectFail = false,
                        EndPoints =
                        {
                            appConfigs.ElasticCacheEndpoint,
                            appConfigs.ElasticCacheReadEndpoint
                        },
                        ClientName = $"{appInfo.ApplicationName}-{Guid.NewGuid()}"
                    };

                    options.InstanceName =
                        $"{appInfo.ApplicationName}-{appInfo.ApplicationVersion}";
                });
            }

            return services;
        }
    }
}
