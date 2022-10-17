

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using System.Security.Policy;
using Template.Aws.Lambda.Infrastructure.Configurations;
using Template.Aws.Lambda.Infrastructure.DataProviders.HttpClients.Polly;

namespace Template.Aws.Lambda.Infrastructure.Extensions
{
    internal static class HttpClientPollyExtensions
    {
        public static IHttpClientBuilder AddPolicyHandler(
            this IHttpClientBuilder builder, HttpClientConfiguration configs)
        {
            builder.Services
                .AddTransient<HttpPollyContextHandler>();

            return builder
                .AddPolicyHandler(CreatePolicies(configs)) // keep order
                .AddHttpMessageHandler<HttpPollyContextHandler>(); // keep order
        }

        public static IAsyncPolicy<HttpResponseMessage> CreatePolicies(
            HttpClientConfiguration configs)
        {
            var policies = new List<IAsyncPolicy<HttpResponseMessage>>()
            {
               CreateTimeOutPolicy(configs.TimeOutPolicy),
               CreateRetryPolicy(configs.RetryPolicy),
               CreateCircuitBreakerPolicy(configs.CircuitBreakerPolicy)
            };

            policies.RemoveAll(p => p is null);

            return Policy.WrapAsync(policies.ToArray());
        }

        private static IAsyncPolicy<HttpResponseMessage> CreateRetryPolicy(
            RetryPolicyConfiguration config)
        {
            if (config is null)
                return null;

            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(
                    retryCount: config.RetryCount,
                    sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(config.SleepDuration),
                    onRetry: (response, wait, context) =>
                    {
                        if (context.TryGetLogger(out var logger))
                        {
                            logger.LogWarning(response?.Exception, "{class} - WaitAndRetryAsync - Trace - " +
                                "WaitMs '{wait}' - StatusCode '{statusCode}'",
                                nameof(Polly), wait.TotalMilliseconds,
                                (int?)response?.Result?.StatusCode ?? -1);
                        }
                    });
        }

        private static IAsyncPolicy<HttpResponseMessage> CreateCircuitBreakerPolicy(
            CircuitBreakerPolicyConfiguration config)
        {
            if (config is null)
                return null;

            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: config.EventsAllowedBeforeBreaking,
                durationOfBreak: TimeSpan.FromMilliseconds(config.DurationOfBreak),
                onBreak: (response, breakDelay, context) =>
                {
                    if (context.TryGetLogger(out var logger))
                    {
                        logger.LogWarning(response?.Exception, "{class} - CircuitBreakerAsync - OnBreak [Open] - " +
                                "BreakMs '{wait}' - StatusCode '{statusCode}'",
                                nameof(Polly), breakDelay.TotalMilliseconds,
                                (int?)response?.Result?.StatusCode ?? -1);
                    }
                },
                onReset: (context) =>
                {
                    if (context.TryGetLogger(out var logger))
                    {
                        logger.LogWarning("{class} - CircuitBreakerAsync - OnReset [Closed]",
                            nameof(Polly));
                    }
                });
        }

        private static IAsyncPolicy<HttpResponseMessage> CreateTimeOutPolicy(
            TimeOutPolicyConfiguration config)
        {
            if (config is null)
                return null;

            return Policy.TimeoutAsync<HttpResponseMessage>(
                .TimeSpan.FromMilliseconds(config.TimeOut),
                onTimeoutAsync: (context, delayed, _, ex) =>
                {
                    if (context.TryGetLogger(out var logger))
                    {
                        logger.LogWarning(ex, "{class} - TimeOutAsync - Trace - " +
                                "Delayed '{delayed}'",
                                nameof(Polly), delayed);
                    }

                    //context.HandleTimeoutMetric(); // of sdk prometheus

                    return Task.CompletedTask;
                });
        }
    }
}
