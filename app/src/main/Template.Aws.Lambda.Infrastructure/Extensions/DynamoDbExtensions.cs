using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Diagnostics;
using Template.Aws.Lambda.Infrastructure.Configurations;

namespace Template.Aws.Lambda.Infrastructure.Extensions
{
    public static class DynamoDbExtensions
    {
        public static IServiceCollection AddAmazonDynamoDb(
            this IServiceCollection services,
            IConfiguration configuration,
            AppConfigurations appConfigs)
        {
            if(Debugger.IsAttached)
            {
                services.TryAddTransient<IContextClientService,
                    ContextClientServiceStub>();
            }
            else
            {
                var dynamoOptions = configuration
                    .GetAWSOptions("DynamoDb");

                var parameters = appConfigs.ContextClient;

                services.AddContextClientService(
                    dynamoOptions, parameters);
            }

            return services;
        }
    }
}
