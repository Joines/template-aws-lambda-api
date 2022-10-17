using Refit;
using System.Threading;
using System.Threading.Tasks;

namespace Template.Aws.Lambda.Infrastructure.DataProviders.HttpClients
{
    public interface IApiASertConsumidacs
    {
        [Get("/Template.Aws.Lambda/clientes/{id_cliente}")]
        //[Headers(HttpClientRequestHeaderValues)]
        Task<Object> GetById([AliasAs("id_cliente")] string id);
    }
}
