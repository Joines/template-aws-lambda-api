using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using Template.Aws.Lambda.Infrastructure.Configurations;

namespace Template.Aws.Lambda.Infrastructure.Extensions
{
    public static class SecretsExtensions
    {
        public static IServiceCollection AddSecrets(this IServiceCollection services,
            AppConfigurations appConfigs,
            IConfiguration configuration)
        {
            services.AddOptions();

            services.TryAddAWSService<IAmazonSecretsManager>(
                configuration.GetAWSOptions("SecretsManager"));

            appConfigs.TokenGenerator.ClientId ??=
                services.GetSecretValue(appConfigs.TokenGenerator.ClientIdPath)
                ?? null;

            appConfigs.TokenGenerator.ClientSecret ??=
                services.GetSecretValue(appConfigs.TokenGenerator.ClientSecretPath)
                ?? null;

            services.Configure<TokenGeneratorConfiguration>(config =>
            {
                config.ClientId = appConfigs.TokenGenerator.ClientId;
                config.ClientSecret = appConfigs.TokenGenerator.ClientSecret;
            });

            return services;
        }

        private static string GetSecretValue(
            this IServiceCollection services, string secretName)
        {
            if (Debugger.IsAttached)
                return null;

            var response = GetSecretResponse(services, secretName);

            return response?.SecretString;
        }

        private static GetSecretValueResponse GetSecretResponse(
            IServiceCollection services, string secretName)
        {
            using var provider = services.BuildServiceProvider();
            using var awsSecretManager =
                provider.GetRequiredService<IAmazonSecretsManager>();

            var request = new GetSecretValueRequest()
            {
                SecretId = secretName
            };

            return awsSecretManager.GetSecretValueAsync(request)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }
    }
}
