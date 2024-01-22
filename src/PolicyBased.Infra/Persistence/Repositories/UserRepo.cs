using Microsoft.EntityFrameworkCore;
using PolicyBased.Infra.Dtos;
using PolicyBased.Infra.Models;
using PolicyBased.Infra.Persistence.Context;
using PolicyDtos = PolicyBased.Infra.Dtos;
namespace PolicyBased.Infra.Persistence.Repositories;

public class UserRepo : IUserRepo
{
    private readonly PolicyTestDBContext _dbContext;

    public UserRepo(PolicyTestDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Subject>> GetAllUsers()
    {
        var allUsers = await _dbContext.Users.Include(u => u.UserRoles).ToListAsync();
        return allUsers.Select(a => new Subject() { UserId = a.UserId.ToString() }).ToList();
    }

    public async Task<List<PolicyDtos.Application>> GetPolicies()
    {
        List<PolicyDtos.Application> apps = new();
        var dbPolicies = await _dbContext.Policies.Include(a => a.AppPolicies).Include(a => a.Application).ToListAsync();
        var appPolicies = await _dbContext.AppPolicies.Include(a => a.Policy).Include(a => a.Permission).Include(a => a.Role).ToListAsync().ConfigureAwait(false);

        var roleUsers = _dbContext.UserRoles.Include(a => a.User).ToList();
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

                var roles = await _dbContext.Roles.Select(a => new PolicyDtos.Role() { Name = a.RoleName, IsSelected = true }).ToListAsync();
                foreach (var r in roles)
                {
                               
                    foreach (var perm in allPerms)
                    {
                        if (appPolicies.Any(a => a.Role.RoleName == r.Name && a.Permission.Name == perm.Name))
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


            app.Policies = policyList.ToArray();
            apps.Add(app);
        }
        return apps;
    }

    public async Task<PolicyDtos.Policy> GetPolicy(int policyId = 1)
    {
        var dbPolicy = await _dbContext.Policies.Include(a => a.AppPolicies).Include(a => a.Application).FirstOrDefaultAsync(a => a.Id == policyId);
        var appPolicies = await _dbContext.AppPolicies.Include(a => a.Policy).Include(a => a.Permission).Include(a => a.Role).ToListAsync().ConfigureAwait(false);

        appPolicies = appPolicies.Where(a => a.PolicyId == policyId && !a.IsDeleted == true).ToList();
        var roleUsers = _dbContext.UserRoles.Include(a => a.User).ToList();
        var allPerms = await _dbContext.Permissions.ToListAsync();


        PolicyDtos.Policy p = new()
        {
            Name = dbPolicy.Name,
            Id = dbPolicy.Id
        };

        var allUsers = await _dbContext.Users.Include(u => u.UserRoles).ToListAsync();
        //var roleNames = await _dbContext.Roles.Select(a => a.RoleName).ToListAsync();

        var roles = await _dbContext.Roles.Select(a => new PolicyDtos.Role() { Name = a.RoleName, IsSelected = true }).ToListAsync();
        foreach (var r in roles)
        {
            //r.Subjects = roleUsers.Where(a => a.Role.RoleName == r.Name).Select(a =>
            //    new Subject() { UserId = a.User.UserId.ToString(), IsSelected = true }).ToList();

            foreach (var u in allUsers)
            {
                if (u.UserRoles.Any(a => a.Role.RoleName == r.Name))
                {
                    r.Subjects.Add(new Subject() { UserName = u.UserName, UserId = u.UserId.ToString(), IsSelected = true });
                }
                else
                {
                    r.Subjects.Add(new Subject() { UserName = u.UserName, UserId = u.UserId.ToString(), IsSelected = false });
                }
            }

            foreach (var perm in allPerms)
            {
                if (appPolicies.Any(a => a.Role.RoleName == r.Name && a.Permission.Name == perm.Name))
                {
                    r.Permissions.Add(new()
                    {
                        Name = perm.Name,
                        IsSelected = true
                    });
                }
                else
                {
                    r.Permissions.Add(new()
                    {
                        Name = perm.Name,
                        IsSelected = false
                    });
                }
            }
        }
        p.Roles = roles;
        return p;
    }

    public async Task SavePolicy(PolicyDtos.Policy policy)
    {
        var appPolicies = await _dbContext.AppPolicies.Include(a => a.Policy)
            .Include(a => a.Permission).Include(a => a.Role).Include(a => a.Role.UserRoles)
            .Where(a => a.PolicyId == policy.Id && !a.IsDeleted == true).ToListAsync().ConfigureAwait(false);

        var allRoles = await _dbContext.Roles.ToListAsync().ConfigureAwait(false);
        var allPerm = await _dbContext.Permissions.ToListAsync().ConfigureAwait(false);
        var allUsers = await _dbContext.Users.ToListAsync();
        appPolicies.ForEach(p =>
        {
            p.IsDeleted = true;
        });

        var roleUsers = await _dbContext.UserRoles.Where(a => !a.IsDeleted == true).ToListAsync();
        roleUsers.ForEach(p =>
        {
            p.IsDeleted = true;
        });

        await _dbContext.SaveChangesAsync();

        var newPolcies = new List<AppPolicy>();
        foreach (var role in policy.Roles.Where(a => a.IsSelected == true).ToList())
        {
            var roleId = allRoles.FirstOrDefault(a => a.RoleName == role.Name).Id;

            // save user's roles
            foreach (var user in role.Subjects.Where(a => a.IsSelected))
            {
                int userId = allUsers.FirstOrDefault(a => a.UserId.ToString() == user.UserId).Id;
                _dbContext.UserRoles.Add(new UserRole() { RoleId = roleId, UserId = userId });
            }

            // save permissions
            foreach (var permission in role.Permissions.Where(p => p.IsSelected).ToList())
            {
                AppPolicy pol = new()
                {
                    PolicyId = policy.Id,
                    RoleId = roleId,
                    PermissionId = allPerm.FirstOrDefault(a => a.Name == permission.Name).Id,
                    IsDeleted = false
                };
                newPolcies.Add(pol);
            }
        }
        await _dbContext.AppPolicies.AddRangeAsync(newPolcies);
        await _dbContext.SaveChangesAsync();
    }
}