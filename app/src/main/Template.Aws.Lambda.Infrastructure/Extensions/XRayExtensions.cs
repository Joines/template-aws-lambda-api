using Amazon.XRay.Recorder.Handlers.System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Template.Aws.Lambda.Infrastructure.Extensions
{
    public static class XRayExtensions
    {
        public static IHttpClientBuilder AddXRayTracing(
            this IHttpClientBuilder builder)
        {
            builder.Services
                .TryAddTransient<HttpClientXRayTracingHandler>();

            return builder
                .AddHttpMessageHandler<HttpClientXRayTracingHandler>();
        }
    }
}
