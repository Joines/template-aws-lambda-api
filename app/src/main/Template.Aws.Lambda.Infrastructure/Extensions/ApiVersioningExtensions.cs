using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace Template.Aws.Lambda.Infrastructure.Extensions
{
    public static class ApiVersioningExtensions
    {
        public const string TagVersionName = "x-itau-version";

        public static IServiceCollection AddApiVersioningTemplate(
            this IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                config.ReportApiVersions = false;
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ApiVersionReader =
                  new QueryStringApiVersionReader(TagVersionName);
            }).AddVersionedApiExplorer();

            return services;
        }
    }
}
