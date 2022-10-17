using Microsoft.Extensions.DependencyInjection;
using Template.Aws.Lambda.Application.Repositories;

namespace Template.Aws.Lambda.Infrastructure.Extensions
{
    public static class AppServicesExtensions
    {
        public static IServiceCollection AddAppServices(
            this IServiceCollection services)
        {
            services.AddScoped<IClientRepository, ClienteRepository>();

            return services;
        }
    }
}
