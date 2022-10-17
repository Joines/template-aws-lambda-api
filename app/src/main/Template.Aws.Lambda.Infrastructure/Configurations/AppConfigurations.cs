namespace Template.Aws.Lambda.Infrastructure.Configurations
{
    public class AppConfigurations
    {
        private const string environmentDev = "development";
        private const string environmentHom = "staging";

        public string? Stage { get; set; }
        public string Environment { get; set; }
        public string? GatewayAwsExpId { get; set; }
        public string? ApiGatewayBaseAdress { get; set; }
        public string? VirtualizacaoBaseUrl { get; set; }
        public string? STSAuthenticationUrl { get; set; }
        public string? STSExtAuthenticationUrltokeninfo { get; set; }
        public string? STSExtAuthenticationUrlVirtual { get; set; }
        public string? ElasticCacheEndpoint { get; set; }
        public string? ElasticCacheReadEndpoint { get; set; }
        public bool IsDevelopment =>
            Environment.Equals(environmentDev, 
                StringComparison.OrdinalIgnoreCase);

        public bool IsStaging =>
            Environment.Equals(environmentHom, 
                StringComparison.OrdinalIgnoreCase);

        public string ApplicationName =>
            Splunk?.ApplicationInfo?.ApplicationName
            ?? string.Empty;

        public string? StreamerEndPoint { get; set; }

        public SplunkConfiguration? Splunk { get; set; }
        public TokenGeneratorConfiguration? TokenGenerator { get; set; }
        public PrometheusConfiguration? Prometheus { get; set; }
        public ICollection<HttpClientConfiguration>? HttpClients { get; set; }
    }
}
