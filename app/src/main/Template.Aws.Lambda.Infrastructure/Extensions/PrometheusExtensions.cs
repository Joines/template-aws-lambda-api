using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using Template.Aws.Lambda.Infrastructure.Configurations;

namespace Template.Aws.Lambda.Infrastructure.Extensions
{
    public static class PrometheusExtensions
    {
        public static IServiceCollection AddPrometheus(
            this IServiceCollection services,
            AppConfigurations appConfigs)
        {
            if (Debugger.IsAttached)
                return services;

            services.AddMetricsPusher(config =>
            {
                config.Job = appConfigs.ApplicationName;
                config.Endpoint = appConfigs.Prometheus.PushGatewayEndpoint;
                config.Labels = new[] { ("Jornada", "corretora") };
            });

            return services;
        }
    }
}
