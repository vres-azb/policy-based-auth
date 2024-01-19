using PolicyBased.Infra.Dtos;
using PolicyBased.Infra.Models;
using PolicyDtos = PolicyBased.Infra.Dtos;

namespace PolicyBased.Infra.Persistence.Repositories
{
    public interface IUserRepo
    {
        Task<List<PolicyDtos.Application>> GetPolicies();
        Task<List<Subject>> GetAllUsers();
    }
}