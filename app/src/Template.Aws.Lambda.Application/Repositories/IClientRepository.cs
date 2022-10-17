using Template.Aws.Lambda.Domain.Entities;

namespace Template.Aws.Lambda.Application.Repositories
{
    public interface IClientRepository
    {
        Task<ICollection<Client>> GetAllClients();
    }
}
