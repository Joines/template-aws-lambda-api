using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Template.Aws.Lambda.Infrastructure.Extensions
{
    public static class MediatorExtensions
    {
        public static IServiceCollection AddMediator(this IServiceCollection services)
        {
            var assembly = AppDomain.CurrentDomain
                .Load("Template.Aws.Lambda.Application");

            services.AddMediatR(assembly);

            return services;
        }
    }
}
