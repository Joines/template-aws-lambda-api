namespace Template.Aws.Lambda.Infrastructure.Configurations
{
    public class SplunkConfiguration
    {
        public ApplicationInfoConfiguration ApplicationInfo { get; set; }
    }

    public class ApplicationInfoConfiguration
    {
        public string ApplicationName { get; set; }
        public string ApplicationVersion { get; set; }
    }
}