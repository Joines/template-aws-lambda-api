using AutoMapper;
using Template.Aws.Lambda.Domain.Entities;
using Template.Aws.Lambda.Domain.Results;

namespace Template.Aws.Lambda.Application.Mappers
{
    public class ClientMapper: Profile
    {
        public ClientMapper()
        {
            _ = CreateMap<ClientResult, Client>();
        }
    }
}
