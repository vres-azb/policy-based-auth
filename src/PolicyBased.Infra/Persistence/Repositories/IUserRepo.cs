using PolicyDtos = PolicyBased.Infra.Dtos;

namespace PolicyBased.Infra.Persistence.Repositories
{
    public interface IUserRepo
    {
        Task<List<PolicyDtos.Application>> GetPolicies();
        Task<List<PolicyDtos.Subject>> GetAllUsers();
        Task<PolicyDtos.Policy> GetPolicy(int policyId);
        Task<bool> SavePolicy(PolicyDtos.Policy policy);
        Task<int> AddNewPolicy(string policyName);
        Task<bool> DeletePolicy(int policyId);
        Task<int> AddNewRole(int policyId, string roleName);
        Task<int> AddNewPermission(int policyId, string permName);
        Task<bool> DeleteRole(int roleId);
        Task<bool> DeletePermission(int permId);
        Task<string> AddNewUser(PolicyDtos.Subject userInfo);
        Task<bool> UpdateUser(PolicyDtos.Subject userInfo);
        Task<bool> DeleteUser(int userId);
    }
}