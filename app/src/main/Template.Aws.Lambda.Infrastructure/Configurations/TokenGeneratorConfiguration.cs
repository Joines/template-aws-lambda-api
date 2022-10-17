namespace Template.Aws.Lambda.Infrastructure.Configurations
{
    public class TokenGeneratorConfiguration
    {
        public string? ClientId { get; set; }
        public string? ClientIdPath { get; set; }
        public string? ClientSecret { get; set; }
        public string? ClientSecretPath { get; set; }
        public short RefreshBeforeExpiration { get; set; }
        public IList<string>? HttpClientsNames { get; set; }
    }
}