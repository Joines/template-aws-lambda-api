using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Template.Aws.Lambda.Infrastructure.Configurations;

namespace Template.Aws.Lambda.Infrastructure.Extensions
{
    public static class AppConfigurationExtensions
    {
        public static AppConfigurations AddAppConfigs(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddConfiguration<AppConfigurations>(configuration)
                .AddConfiguration<object>(configuration,
                "ConfigNoAppSettings");

            using var provider = services.BuildServiceProvider();

            return provider.GetRequiredService<AppConfigurations>();
        }

        private static IServiceCollection AddConfiguration<TParameterType>(
            this IServiceCollection services,
            IConfiguration configuration,
            string? sectionName = null)
            where TParameterType : class, new()
        {
            var section = string.IsNullOrEmpty(sectionName) ?
                configuration :
                configuration.GetSection(sectionName);

            services.Configure<TParameterType>(section);
            services.AddScoped(c =>
                c.GetRequiredService<IOptionsMonitor<TParameterType>>()
                .CurrentValue);

            return services;
        }
    }
}
