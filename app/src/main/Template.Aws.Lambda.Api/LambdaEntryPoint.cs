using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Net.Sockets;
using Template.Aws.Lambda.AspnetServer.Sdk;
using Template.Aws.Lambda.Infrastructure.Extensions;

namespace Template.Aws.Lambda.Api
{
    public class LambdaEntryPoint: 
        GatewayProxyFunction<Startup>
    {
        protected override void Init(IHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                var envName = context.HostingEnvironment
                    .EnvironmentName;

                config.AddJsonFile($"appsetting.json",
                          optional: false, reloadOnChange: true)
                      .AddJsonFile($"appsettings.{envName}.json",
                          optional: false, reloadOnChange: true)
                      .AddConfigurationSystemManager();
            });
        }
    }
}
