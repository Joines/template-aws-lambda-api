using CorrelationId.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Template.Aws.Lambda.Infrastructure.Extensions
{
    public static class CorrelationIdExtensions
    {
        public const string CorrelationHeaderName = "x-template-correlationId";

        public static IServiceCollection AddCorrelation(
            this IServiceCollection services)
        {
            services.AddDefaultCorrelationId(configure =>
            {
                configure.EnforceHeader = false;
                configure.AddToLoggingScope = true;
                configure.IncludeInResponse = true;
                configure.RequestHeader = CorrelationHeaderName;
                configure.ResponseHeader = CorrelationHeaderName;
                configure.CorrelationIdGenerator =
                    () => Guid.NewGuid().ToString();
            });

            return services;
        }
    }
}
