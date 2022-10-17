using Amazon.XRay.Recorder.Core;
using Amazon.XRay.Recorder.Handlers.AwsSdk;
using CorrelationId;
using CorrelationId.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Diagnostics;
using Template.Aws.Lambda.Api.Middlewares;
using Template.Aws.Lambda.Infrastructure.Configurations;
using Template.Aws.Lambda.Infrastructure.Extensions;

namespace Template.Aws.Lambda.Api
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private AppConfigurations appConfigs;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;

            AWSXRayRecorder.InitializeInstance(configuration);
            AWSSDKHandler.RegisterXRayForAllServices();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            appConfigs = services.AddAppConfigs(configuration);

            services.AddSecrets(appConfigs, configuration);

            services.AddTokenGenerator(appConfigs);
            services.AddTokenAuthentication(appConfigs, configuration);

            services.AddMediator();
            services.AddAutoMapper();
            services.AddAppServices();
            services.AddHateoasLinks();
            services.AddApiVersioningTemplate();
            services.AddSwaggerGenerator();
            services.AddHttpClients(appConfigs);
            services.AddAmazonDynamoDb(configuration, appConfigs);
            //services.AddloggingAWS(configuration);

            services.AddCorrelationId();
            services.AddCache(appConfigs);

            services.AddControllers(options =>
            {
                options.AllowEmptyInputInBodyModelBinding = true;
            })
            .AddFluentValidation()
            .AddNewtonsoftJson(options =>
                ConfigureJsonSettings(options.SerializerSettings))
            .ConfigureApiBehaviorOptions(options =>
                options.SuppressConsumesConstraintForFormFileParameters = true);

            services.ConfigureErrorHandlerServices();
        }

        public static void ConfigureJsonSettings(
            JsonSerializerSettings settings)
        {
            settings.ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };

            settings.Converters.Add(new StringEnumConverter());
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!(env?.IsProduction() ?? true))
                app.RegisterSwagger();

            app.UseCorrelationId();
            app.UseXRay(appConfigs.ApplicationName);

            app.UseRouting();
            //app.UseHttpMetrics(); // ordem necessária, após UseRouting
            app.UseCustomGlobalErrorHandler();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //if (Debugger.IsAttached)
                //    endpoints.MapMetrics();

                endpoints.MapControllers();
            });
        }
    }
}