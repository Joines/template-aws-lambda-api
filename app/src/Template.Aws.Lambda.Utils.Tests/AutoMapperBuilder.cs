using AutoMapper;
using Template.Aws.Lambda.Application.Mappers;

namespace Template.Aws.Lambda.Utils.Tests
{
    public class AutoMapperBuilder
    {
        private AutoMapperBuilder() { }

        public static IMapper Build<T>() where T : Profile, new() =>
            Build(typeof(T));

        public static IMapper Build<T1, T2>()
            where T1 : Profile, new()
            where T2 : Profile, new() =>
                Build(typeof(T1), typeof(T2));

        public static IMapper Build<T1, T2, T3>()
            where T1 : Profile, new()
            where T2 : Profile, new()
            where T3 : Profile, new() =>
            Build(typeof(T1), typeof(T2), typeof(T3));

        private static IMapper Build(params Type[] types)
        {
            var configuration = new MapperConfiguration(config =>
            {
                config.AddProfile<ClientMapper>();
                config.AllowNullCollections = true;

                if (types != null)
                {
                    foreach (var type in types)
                        config.AddProfile(type);
                }
            });

            configuration.AssertConfigurationIsValid();

            return configuration.CreateMapper();
        }
    }
}
