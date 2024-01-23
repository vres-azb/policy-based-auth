using Microsoft.EntityFrameworkCore;
using PolicyBased.Infra.Models;
using PolicyBased.Infra.Persistence.Context;
using Policy = PolicyBased.Infra.Models.Policy;
using PolicyDtos = PolicyBased.Infra.Dtos;
namespace PolicyBased.Infra.Persistence.Repositories;

public class UserRepo : IUserRepo
{
    private readonly PolicyTestDBContext _dbContext;

    public UserRepo(PolicyTestDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<PolicyDtos.Subject>> GetAllUsers()
    {
        var allUsers = await _dbContext.Users.Include(u => u.UserRoles).ToListAsync();
        return allUsers.Select(a => new PolicyDtos.Subject() { Id = a.Id, UserName = a.UserName, Email = a.Email, UserId = a.UserId.ToString() }).ToList();
    }

    public async Task<List<PolicyDtos.Application>> GetPolicies()
    {
        List<PolicyDtos.Application> apps = new();
        var dbPolicies = await _dbContext.Policies.Include(a => a.AppPolicies).Include(a => a.Application).Where(a => !a.IsDeleted).ToListAsync();
        var appPolicies = await _dbContext.AppPolicies.Where(a => !a.IsDeleted).ToListAsync().ConfigureAwait(false);

        var roleUsers = _dbContext.UserRoles.Where(a => !a.IsDeleted).Include(a => a.User).ToList();
        var allPerms = await _dbContext.Permissions.Where(a => !a.IsDeleted).ToListAsync();
        var distApps = _dbContext.Applications.Where(a => !a.IsDeleted).ToList().Select(a => new PolicyDtos.Application()
        {
            Id = a.Id,
            Name = a.Name,
        });

        var allRoles = await _dbContext.Roles.Where(a => !a.IsDeleted)
                .Select(a => new PolicyDtos.Role() { RoleId = a.Id, Name = a.RoleName, PolicyId = a.PolicyId }).ToListAsync();

        foreach (var app in distApps)
        {
            List<PolicyDtos.Policy> policyList = new();
            foreach (var dbPolicy in dbPolicies.Where(a => a.ApplicationId == app.Id))
            {
                var appPolicy = appPolicies.Where(a => a.PolicyId == dbPolicy.Id).ToList();
                PolicyDtos.Policy p = new()
                {
                    Name = dbPolicy.Name,
                    Id = dbPolicy.Id
                };

                var roles = allRoles.Where(a => a.PolicyId == dbPolicy.Id).ToList();
                foreach (var r in roles)
                {
                    foreach (var perm in allPerms.Where(a => a.PolicyId == dbPolicy.Id))
                    {
                        if (appPolicy.Any(a => a.RoleId == r.RoleId && a.PermissionId == perm.Id))
                        {
                            r.Permissions.Add(new()
                            {
                                Name = perm.Name,
                                IsSelected = true
                            });
                        }
                    }
                }
                p.Roles = roles;

                policyList.Add(p);
            }
            app.Policies = policyList;
            apps.Add(app);
        }
        return apps;
    }

    public async Task<PolicyDtos.Policy> GetPolicy(int policyId)
    {
        var dbPolicy = await _dbContext.Policies.Include(a => a.AppPolicies).Include(a => a.Application).FirstOrDefaultAsync(a => a.Id == policyId);
        var appPolicies = await _dbContext.AppPolicies.Include(a => a.Policy).Include(a => a.Permission)
            .Include(a => a.Role).Where(a => !a.IsDeleted).ToListAsync().ConfigureAwait(false);

        appPolicies = appPolicies.Where(a => a.PolicyId == policyId && !a.IsDeleted).ToList();
        var roleUsers = _dbContext.UserRoles.Where(a => !a.IsDeleted).Include(a => a.User).ToList();
        var allPerms = await _dbContext.Permissions.Where(a => a.PolicyId == policyId && !a.IsDeleted).ToListAsync();
        PolicyDtos.Policy p = new()
        {
            Name = dbPolicy.Name,
            Id = dbPolicy.Id
        };

        var allUsers = await _dbContext.Users.Where(a => !a.IsDeleted).ToListAsync();
        var roles = await _dbContext.Roles.Where(a => a.PolicyId == policyId && !a.IsDeleted)
                .Select(a => new PolicyDtos.Role() { RoleId = a.Id, Name = a.RoleName }).ToListAsync();
        foreach (var r in roles)
        {
            foreach (var u in allUsers)
            {
                bool isExists = u.UserRoles.Any(a => a.RoleId == r.RoleId);
                r.Subjects.Add(new PolicyDtos.Subject()
                {
                    UserName = u.UserName,
                    UserId = u.UserId.ToString(),
                    Email = u.Email,
                    Id = u.Id,
                    IsSelected = isExists
                });
            }

            foreach (var perm in allPerms)
            {
                bool isExists = (appPolicies.Any(a => a.RoleId == r.RoleId && a.Permission.Id == perm.Id));
                r.Permissions.Add(new()
                {
                    Name = perm.Name,
                    Id = perm.Id,
                    IsSelected = isExists
                });
            }
        }
        p.Roles = roles;
        return p;
    }

    public async Task SavePolicy(PolicyDtos.Policy policy)
    {
        var appPolicies = await _dbContext.AppPolicies.Include(a => a.Policy)
            .Include(a => a.Permission).Include(a => a.Role).Include(a => a.Role.UserRoles)
            .Where(a => a.PolicyId == policy.Id && !a.IsDeleted).ToListAsync().ConfigureAwait(false);

        var allRoles = await _dbContext.Roles.Where(a => a.PolicyId == policy.Id && !a.IsDeleted).ToListAsync().ConfigureAwait(false);
        var allPerm = await _dbContext.Permissions.Where(a => a.PolicyId == policy.Id && !a.IsDeleted).ToListAsync().ConfigureAwait(false);
        var allUsers = await _dbContext.Users.ToListAsync();
        appPolicies.ForEach(p =>
        {
            p.IsDeleted = true;
        });

        var roleUsers = _dbContext.UserRoles.ToList()
                .Where(a => !a.IsDeleted == true && policy.Roles.Any(r => r.RoleId == a.RoleId)).ToList();
        roleUsers.ForEach(p =>
        {
            p.IsDeleted = true;
        });

        await _dbContext.SaveChangesAsync();

        var newPolcies = new List<AppPolicy>();
        foreach (var role in policy.Roles)
        {
            // save user's roles
            foreach (var user in role.Subjects.Where(a => a.IsSelected))
            {
                _dbContext.UserRoles.Add(new UserRole() { RoleId = role.RoleId, UserId = user.Id });
            }

            // save permissions
            foreach (var permission in role.Permissions.Where(p => p.IsSelected).ToList())
            {
                AppPolicy pol = new()
                {
                    PolicyId = policy.Id,
                    RoleId = role.RoleId,
                    PermissionId = permission.Id,
                    IsDeleted = false
                };
                newPolcies.Add(pol);
            }
        }
        await _dbContext.AppPolicies.AddRangeAsync(newPolcies);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<int> AddNewPolicy(string policyName)
    {
        Policy policy = new()
        {
            Name = policyName,
            ApplicationId = 1
        };
        await _dbContext.AddAsync(policy);
        await _dbContext.SaveChangesAsync();
        return policy.Id;
    }

    public async Task<bool> DeletePolicy(int policyId)
    {
        var policy = await _dbContext.Policies.SingleOrDefaultAsync(p => p.Id == policyId);
        if (policy != null)
        {
            policy.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
        }
        return true;
    }

    public async Task<int> AddNewRole(int policyId, string roleName)
    {
        Role role = new()
        {
            RoleName = roleName,
            PolicyId = policyId
        };
        await _dbContext.AddAsync(role);
        await _dbContext.SaveChangesAsync();
        return role.Id;
    }

    public async Task<int> AddNewPermission(int policyId, string permName)
    {
        Permission perm = new()
        {
            Name = permName,
            PolicyId = policyId
        };
        await _dbContext.AddAsync(perm);
        await _dbContext.SaveChangesAsync();
        return perm.Id;
    }

    public async Task<bool> DeleteRole(int roleId)
    {
        var role = await _dbContext.Roles.FirstOrDefaultAsync(a => a.Id == roleId);
        if (role != null)
        {
            role.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<bool> DeletePermission(int permId)
    {
        var perm = await _dbContext.Permissions.FirstOrDefaultAsync(a => a.Id == permId);
        if (perm != null)
        {
            perm.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
            return true;
        }
        return false;
    }

}