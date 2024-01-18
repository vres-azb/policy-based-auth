using Microsoft.EntityFrameworkCore;
using PolicyBased.Infra.Persistence.Context;
using PolicyDtos = PolicyBased.Contracts;
namespace PolicyBased.Infra.Persistence.Repositories
{

    public class UserRepo : IUserRepo
    {
        private readonly PolicyTestDBContext _dbContext;

        public UserRepo(PolicyTestDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<PolicyDtos.Application>> GetPolicies()
        {
            List<PolicyDtos.Application> apps = new();
            var dbPolicies = await _dbContext.Policies.Include(a => a.AppPolicies).Include(a => a.Application).ToListAsync();
            var appPolicies = await _dbContext.AppPolicies.Include(a => a.Policy).Include(a => a.Permission).Include(a => a.Role).ToListAsync().ConfigureAwait(false);

            var allPerms = await _dbContext.Permissions.ToListAsync();
            var distApps = _dbContext.Applications.ToList().Select(a => new PolicyDtos.Application()
            {
                Id = a.Id,
                Name = a.Name,
            });

            foreach (var app in distApps)
            {
                List<PolicyDtos.Policy> policyList = new();
                foreach (var dbPolicy in dbPolicies)
                {
                    PolicyDtos.Policy p = new()
                    {
                        Name = dbPolicy.Name,
                        Id = dbPolicy.Id
                    };
                    var perAppPol = appPolicies.Where(a => a.PolicyId == dbPolicy.Id);
                    var roleNames = perAppPol.Select(a => a.Role.RoleName).Distinct().ToList();
                    var roles = roleNames.Select(a => new PolicyDtos.Role() { Name = a }).ToList();

                    foreach (var perm in allPerms)
                    {
                        PolicyDtos.Permission prm = new PolicyDtos.Permission()
                        {
                            Name = perm.Name,
                            Roles = perm.AppPolicies.Where(a => a.PermissionId == perm.Id).Select(a => a.Role.RoleName).ToList()
                        };
                        p.Permissions.Add(prm);
                    }
                    p.Roles = roles;
                    policyList.Add(p);
                }
                app.Policies = policyList.ToArray();
                apps.Add(app);
            }
            return apps;
        }
    }
}
