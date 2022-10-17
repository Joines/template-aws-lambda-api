using Amazon.SecretsManager.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using Template.Aws.Lambda.Infrastructure.Configurations;

namespace Template.Aws.Lambda.Infrastructure.Extensions
{
    public static class OAuth2Extensions
    {
        public static IServiceCollection AddTokenAuthentication(
            this IServiceCollection services,
            AppConfigurations appConfigs,
            IConfiguration configuration)
        {
            services.AddTokenAuthentication(
                new AuthenticationOptions()
                {
                    STSExtAuthenticationUrltokeninfo = appConfigs.STSExtAuthenticationUrltokeninfo,
                    STSExtAuthenticationUrlVirtual = appConfigs.STSExtAuthenticationUrlVirtual,
                    Stage = appConfigs.Stage,
                    MBI = new MbiOptions()
                    {
                        GatewayAwsExpId = appConfigs.GatewayAwsExpId
                    }
                });

            services.AddUserDataServices(configuration,
                new UserDataOptions(
                    appConfigs.STSAuthenticationUrl,
                    appConfigs.ApiGatewayBaseAdress,
                    appConfigs.FH5UrlSuffix,
                    appConfigs.FH5DefaultHeaders,
                    appConfigs.TokenGenerator.ClientId,
                    appConfigs.TokenGenerator.ClientSecret));

            return services;
        }

        public static IServiceCollection AddTokenGenerator(
            this IServiceCollection services, AppConfigurations appConfigs)
        {
            var settings = new TokenGeneratorSettings(
                tokenUrl: appConfigs.STSAuthenticationUrl,
                clientId: appConfigs.TokenGenerator.ClientId,
                authenticatedHttpClientsNames:
                    appConfigs.TokenGenerator.HttpClientsNames,
                clientCredentialsTransmissionType:
                    ClientCredentialTransmission.Body,
                refreshTimeBeforeTokenExpiration:
                    appConfigs.TokenGenerator.RefreshBeforeExpiration);

            services.AddAccessTokenManager(options =>
            {
                var tokenRequest = new ClientCredentialsTokenRequest()
                {
                    Address = settings.TokenUrl,
                    GrantType = "client_credentials",
                    ClientId = settings.ClientId,
                    ClientSecret = appConfigs.TokenGenerator.ClientSecret,
                    ClientCredentialStyle = ClientCredentialStyle.PostBody
                };

                options.Client.Clients
                    .Add(settings.TokenServiceName, tokenRequest);

                options.Client.CacheLifeTimeBuffer =
                    settings.RefreshTimeBeforeTokenExpiration;
            })
            .ConfigureBackchannelHttpClient(client =>
            {
                client.DefaultRequestHeaders.Add(
                    HttpClientRequestHeaderValues.HeaderName, "/api/oauth/token");
            })
            .AddCorrelationIdForwarding()
            .AddPolicyHandler(HttpClientConfiguration.Default)
            .AddXRayTracing()
            .AddHttpClientMetrics();

            foreach(var clientName in settings.AuthenticatedHttpClientsNames)
            {
                services.AddClientAccessTokenClient(
                    clientName, settings.TokenServiceName);
            }

            return services;
        }
    }
}
