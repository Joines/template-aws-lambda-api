using Microsoft.Extensions.DependencyInjection;

namespace Template.Aws.Lambda.Infrastructure.Extensions
{
    public static class FluentValidationExtensions
    {
        public static IMvcBuilder AddFluentValidation(
            this IMvcBuilder builder)
        {
            //builder.AddFluentValidation(fv =>
            //{
            //
            //});

            return builder;
        }
    }
}
