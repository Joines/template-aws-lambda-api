using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Template.Aws.Lambda.Application.Mappers;

namespace Template.Aws.Lambda.Infrastructure.Extensions
{
    public static class AutoMapperExtensions
    {
        public static IServiceCollection AddAutoMapper(
            this IServiceCollection services)
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ClientMapper>();
                cfg.AllowNullCollections = true;
            });

            configuration.AssertConfigurationIsValid();
            services.AddSingleton(configuration.CreateMapper());

            return services;
        }
    }
}
