using System.Security.Claims;

namespace FakeRepos
{
    public interface IFakeRepository
    {
        ClaimsPrincipal RetrieveUser(string userName);
        BusinessApplications RetrieveBusinessApps();
    }

    public class FakeRepository : IFakeRepository
    {
        public ClaimsPrincipal RetrieveUser(string userName)
        {
            ClaimsPrincipal result = null;

            if (!string.IsNullOrWhiteSpace(userName))
            {
                var claims = new List<Claim>();

                if (userName == "alice")
                {
                    claims = new List<Claim>
                    {
                        new Claim("sub", "1"),
                        new Claim("name", "Alice"),
                        new Claim("tenantId","T1")
                    };
                }
                else if (userName == "bob")
                {
                    claims = new List<Claim>
                    {
                        new Claim("sub", "2"),
                        new Claim("name", "Bob"),
                        new Claim("tenantId","T1")
                    };
                }
                else
                {
                    claims = new List<Claim>
                    {
                        new Claim("sub", "333"),
                        new Claim("name", userName),
                        new Claim("tenantId","Tenant 3"),
                        //new Claim("canPerform", "maybe"),
                        new Claim("role", "Tenant/Vendor User"),
                        new Claim("role_src", "This claim comes from The OIDC provider.")
                    };
                }

                var id = new ClaimsIdentity(claims, "password", "name", "role");
                result = new ClaimsPrincipal(id);
            }

            return result;
        }

        public BusinessApplications RetrieveBusinessApps()
        {
            BusinessApplications result = new BusinessApplications
            {
                Applications = new Application[]
                {
                    new Application
                    {
                        Id = 1,
                        Name = "My App 1",
                        Policies = new Policy[]
                        {
                            new Policy
                            {
                                Id= 1,
                                Name="My Policy # 1",
                                Roles = new List<Role>
                                {
                                    new Role
                                    {
                                        Name="Manager",
                                        Subjects= new List<string>
                                        {
                                            "1",
                                        },
                                        IdentityRoles= new List<string>
                                        {
                                            "Vero User",
                                        }
                                    },
                                    new Role
                                    {
                                        Name="Staff",
                                        Subjects= new List<string>
                                        {
                                            "2",
                                        },
                                        IdentityRoles= new List<string>
                                        {
                                            "Vero User",
                                        }
                                    }
                                },
                                Permissions = new List<Permission>
                                {
                                    new Permission
                                    {
                                        Name = "CanViewOrders",
                                        Roles= new List<string>{
                                            "Manager",
                                            "Staff"
                                        }
                                    },
                                    new Permission
                                    {
                                        Name = "CanDeleteOrders",
                                        Roles= new List<string>{
                                            "Manager"
                                        }
                                    },
                                    new Permission
                                    {
                                        Name = "CanExportOrderToPDF",
                                        Roles= new List<string>{
                                            "Manager",
                                            "Staff"
                                        }
                                    }
                                }
                            },
                            new Policy
                            {
                                Id= 2,
                                Name="My Policy # 2",
                                Roles = new List<Role>
                                {
                                    new Role
                                    {
                                        Name="Appraiser",
                                        Subjects= new List<string>
                                        {
                                            "333",
                                        },
                                        IdentityRoles= new List<string>
                                        {
                                            "Another Tenant User",
                                        }
                                    }
                                },
                                Permissions = new List<Permission>
                                {
                                    new Permission
                                    {
                                        Name = "CanExportOrderToPDF",
                                        Roles= new List<string>{
                                            "Manager",
                                            "Staff"
                                        }
                                    },
                                    new Permission
                                    {
                                        Name = "CanViewOrders",
                                        Roles= new List<string>{
                                            "Staff"
                                        }
                                    },
                                    new Permission
                                    {
                                        Name = "CanDeleteMyOwnOrders",
                                        Roles= new List<string>{
                                            "Manager"
                                        }
                                    },
                                }
                            }
                        }
                    },
                    new Application
                    {
                        Id = 2,
                        Name = "My App # 2",
                        Policies = new Policy[]
                        {
                            new Policy
                            {
                                Id= 3,
                                Name="My Policy # 3",
                                Roles = new List<Role>
                                {
                                    new Role
                                    {
                                        Name="Manager",
                                        Subjects= new List<string>
                                        {
                                            "1",
                                        },
                                        IdentityRoles= new List<string>
                                        {
                                            "Vero User",
                                        }
                                    },
                                    new Role
                                    {
                                        Name="Appraiser",
                                        Subjects= new List<string>
                                        {
                                            "2",
                                        },
                                        IdentityRoles= new List<string>
                                        {
                                            "Vero User",
                                        }
                                    }
                                },
                                Permissions = new List<Permission>
                                {
                                    new Permission
                                    {
                                        Name = "CanExportOrderToPDF",
                                        Roles= new List<string>{
                                            "Manager",
                                            "Staff"
                                        }
                                    },
                                    new Permission
                                    {
                                        Name = "CanInviteMembers",
                                        Roles= new List<string>{
                                            "Manager"
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            return result;
        }
    }
}