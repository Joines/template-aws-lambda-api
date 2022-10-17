using MediatR;
using Template.Aws.Lambda.Application.Models;

namespace Template.Aws.Lambda.Application.Queries
{
    public class GetAllClientQuery: IRequest<GetClientResponse>
    {
    }
}
