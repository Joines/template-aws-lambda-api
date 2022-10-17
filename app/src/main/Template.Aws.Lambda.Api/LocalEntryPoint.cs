using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Template.Aws.Lambda.Api;
using Template.Aws.Lambda.Infrastructure.Extensions;

public class LocalEntryPoint
{
    protected LocalEntryPoint() { }

    public static async Task Main(string[] args) =>
        await CreateHostBuilder(args)
        .Build()
        .RunAsync();

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((context, config) =>
        {
            var envName = context.HostingEnvironment
                .EnvironmentName;

            config.AddJsonFile($"appsetting.json",
                optional: false, reloadOnChange: true)
                  .AddJsonFile($"appsettings.{envName}.json",
                optional: false, reloadOnChange: true);
        })
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
}