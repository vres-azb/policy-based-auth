using PolicyDtos = PolicyBased.Infra.Dtos;

namespace PolicyBased.Infra.Persistence.Repositories
{
    public interface IUserRepo
    {
        Task<List<PolicyDtos.Application>> GetPolicies();
        Task<List<PolicyDtos.Subject>> GetAllUsers();
        Task<PolicyDtos.Policy> GetPolicy(int policyId);
        Task SavePolicy(PolicyDtos.Policy policy);
    }
}