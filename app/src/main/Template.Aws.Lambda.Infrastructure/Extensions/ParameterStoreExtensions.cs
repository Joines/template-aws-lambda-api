using Microsoft.Extensions.Configuration;

namespace Template.Aws.Lambda.Infrastructure.Extensions
{
    public static class ParameterStoreExtensions
    {
        public static IConfigurationBuilder AddConfigurationSystemManager(
            this IConfigurationBuilder builder)
        {
            var configRoot = builder.Build();
            var awsCommonPaths = configRoot
                .GetSection("AwsCommonParametersPaths")
                ?.Get<ICollection<string>>()
                ?? Enumerable.Empty<string>();

            foreach(var path in awsCommonPaths)
            {
                builder = builder.AddSystemsManager(
                    path: path,
                    optional: true);
            }

            return builder;
        }
    }
}
