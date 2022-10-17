using Microsoft.Extensions.Logging;
using Polly;

namespace Template.Aws.Lambda.Infrastructure.Extensions
{
    internal static class PollyContextExtensions
    {
        public const string PollyContextLoggerKey = "context_logger_key";

        public static bool TryGetLogger(this Context context,
            out ILogger logger)
        {
            return TryGet(context,
                PollyContextLoggerKey, out logger);
        }

        private static bool TryGet<T>(Context context,
            string key, out T value) 
            where T : class
        {
            if(context != null &&
                context.TryGetValue(key, out var obj) &&
                obj is T _value)
            {
                value = _value;
                return true;
            }

            value = default;
            return false;
        }
    }
}
