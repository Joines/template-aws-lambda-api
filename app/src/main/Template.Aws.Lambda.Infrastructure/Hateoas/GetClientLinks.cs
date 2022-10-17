using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Net;
using Template.Aws.Lambda.Application.Models;

namespace Template.Aws.Lambda.Infrastructure.Hateoas
{
    internal static class GetClientLinks
    {
        private const string indexPath = "{0}clientes";

        public static IServiceCollection AddGetClientLinks(
            this IServiceCollection services)
        {
            services.AddHateoas<GetClientResponse>(HttpStatusCode.OK)
                .AddLink((builder, link) =>
                {
                    var href = string.Format(indexPath, builder.Stage);

                    link.Method = "POST";
                    link.Relation = "template-client";
                    link.RelationPath = href;
                }, condition:
                    builder => builder)
        }



    }
}
