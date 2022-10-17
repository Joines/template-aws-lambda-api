namespace Template.Aws.Lambda.Infrastructure.Configurations
{
    public class HttpClientConfiguration
    {
        public string Name { get; set; }
        public string BaseAdress { get; set; }
        public bool VirtualizacaoDevEnabled { get; set; }
        public string VirtualizacaoDevPath { get; set; }
        public bool VirtualizacaoHomEnabled { get; set; }
        public string VirtualizacaoHomPath { get; set; }

        public RetryPolicyConfiguration RetryPolicy { get; set; }
        public CircuitBreakerPolicyConfiguration CircuitBreakerPolicy { get; set; }
        public TimeOutPolicyConfiguration TimeOutPolicy { get; set; }

        public bool VirtualizacaoDev =>
            VirtualizacaoDevEnabled &&
            !string.IsNullOrEmpty(VirtualizacaoDevPath);

        public bool VirtualizacaoHom =>
            VirtualizacaoHomEnabled &&
            !string.IsNullOrEmpty(VirtualizacaoHomPath);

        public static HttpClientConfiguration Default { get; } = new HttpClientConfiguration()
        {
            Name = nameof(Default),
            VirtualizacaoDevEnabled = false,
            VirtualizacaoHomEnabled = false,
            RetryPolicy = new RetryPolicyConfiguration()
            {
                RetryCount = 3,
                SleepDuration = 100
            },
            CircuitBreakerPolicy = new CircuitBreakerPolicyConfiguration()
            {
                DurationOfBreak = 5000,
                EventsAllowedBeforeBreaking = 16
            },
            TimeOutPolicy = new TimeOutPolicyConfiguration()
            {
                TimeOut = 5000
            }
        };

    }

    public class TimeOutPolicyConfiguration
    {
        public short TimeOut { get; set; }
    }

    public class CircuitBreakerPolicyConfiguration
    {
        public short EventsAllowedBeforeBreaking { get; set; }
        public short DurationOfBreak { get; set; }
    }

    public class RetryPolicyConfiguration
    {
        public short RetryCount { get; set; }
        public short SleepDuration { get; set; }
    }
}