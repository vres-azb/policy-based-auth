using PolicyBased.Infra.Models;
using PolicyDtos = PolicyBased.Contracts;

namespace PolicyBased.Infra.Persistence.Repositories
{
    public interface IUserRepo
    {
        Task<List<PolicyDtos.Application>> GetPolicies();
    }
}