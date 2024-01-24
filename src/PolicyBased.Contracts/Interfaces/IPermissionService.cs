using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolicyBased.Contracts.Services;

public interface IPermissionService
{
    Task<PolicyResult> GetPermissions(string userId);
}
