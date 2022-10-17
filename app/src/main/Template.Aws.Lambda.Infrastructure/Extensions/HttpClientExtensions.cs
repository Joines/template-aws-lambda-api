using Amazon.XRay.Recorder.Core.Sampling;
using CorrelationId.HttpClient;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Template.Aws.Lambda.Infrastructure.Configurations;
using Template.Aws.Lambda.Infrastructure.DataProviders.HttpClients;

namespace Template.Aws.Lambda.Infrastructure.Extensions
{
    public static class HttpClientExtensions
    {
        public const string ApiKeyHeaderName = "x-template-apikey";
        public const string AwsGatewayIdHeaderNmer = "x-apigtw-api-id";

        public static IServiceCollection AddHttpClients(
            this IServiceCollection services, AppConfigurations appConfigs)
        {
            services.AddClient<IApiASertConsumidacs>("ExpClient",
                appConfigs, appConfigs.GatewayAwsExpId);

            return services;
        }

        private static IServiceCollection AddClient<TClient>(
            this IServiceCollection services,
            string name,
            AppConfigurations appConfigs, string awsGtwId = null)
            where TClient : class
        {
            var httpConfigs = appConfigs.HttpClients
                .Single(s => string.Equals(s.Name, name));

            var uri = GetBaseAddress(appConfigs, httpConfigs);

            services.
                AddHttpClient<TClient>(httpConfigs.Name, client =>
                {
                    client.BaseAddress = uri;
                    client.DefaultRequestHeaders.Add(
                        ApiKeyHeaderName,
                        appConfigs.TokenGenerator.ClientId);

                    if (awsGtwId != null)
                    {
                        client.DefaultRequestHeaders.Add(AwsGatewayIdHeaderNmer, awsGtwId);
                    }
                })
                .ConfigurePrimaryHttpMessageHandler(ConfiguraPrimaryHandler)
                .AddCorrelationIdForwarding()
                .AddPolicyHandler(httpConfigs)
                .AddTypedClient(client =>
                    RestService.For<TClient>(client, RefitJsonSettings)));

            return services;
        }

        private static HttpMessageHandler ConfiguraPrimaryHandler()
        {
            return new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip
            };
        }

        public static Uri GetBaseAddress(AppConfigurations appConfigs,
            HttpClientConfiguration httpConfigs)
        {
            if(appConfigs.IsDevelopment && httpConfigs.VirtualizacaoDev)
            {
                return new Uri(string.Concat(
                    appConfigs.VirtualizacaoBaseUrl,
                    httpConfigs.VirtualizacaoDevPath));
            }

            if (appConfigs.IsStaging && httpConfigs.VirtualizacaoHom)
            {
                return new Uri(string.Concat(
                    appConfigs.VirtualizacaoBaseUrl,
                    httpConfigs.VirtualizacaoDevPath));
            }

            var baseAddress = httpConfigs.BaseAdress switch
            {
                nameof(AppConfigurations.ApiGatewayBaseAdress) =>
                    string.Concat(appConfigs.ApiGatewayBaseAdress,
                        appConfigs.GatewayAwsExpId),

                _ => throw new ArgumentException("invalid base address", nameof(appConfigs))
            };

            return new Uri(baseAddress);
        }

        private static RefitSettings RefitJsonSettings()
        {
            var jsonOptions = new JsonSerializerOptions()
            {
                IgnoreNullValues = true,
                WriteIndented = false,
                PropertyNameCaseInsensitive = true,
            };

            jsonOptions.Converters
                .Add(new JsonStringEnumConverter());

            var settings = new RefitSettings()
            {
                ContentSerializer = new SystemTextJsonContentSerializer(jsonOptions)
            };

            return settings;
        }
        
    }
}
