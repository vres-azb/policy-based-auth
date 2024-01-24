using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FakeRepos;
using PolicyBased.Contracts;
using PolicyBased.Contracts.Services;

namespace PolicyServer.Runtime.Client
{
    /// <summary>
    /// PolicyServer client
    /// </summary>
    public class PolicyServerRuntimeClient : IPolicyServerRuntimeClient
    {
        private readonly BusinessApplications _businessApplications;
        private readonly IPermissionService _permissionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PolicyServerRuntimeClient"/> class.
        /// </summary>
        /// <param name="applications">The Business pplications.</param>
        /// <param name="permissionService"></param>
        public PolicyServerRuntimeClient(BusinessApplications applications, IPermissionService permissionService)
        {
            this._businessApplications =  applications;
            _permissionService = permissionService;
        }

        /// <summary>
        /// Determines whether the user is in a role.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        public async Task<bool> IsInRoleAsync(ClaimsPrincipal user, string role)
        {
            var policy = await EvaluateAsync(user);
            return policy.Roles.Contains(role);
        }

        /// <summary>
        /// Determines whether the user has a permission.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="permission">The permission.</param>
        /// <returns></returns>
        public async Task<bool> HasPermissionAsync(ClaimsPrincipal user, string permission)
        {
            var policy = await EvaluateAsync(user);
            return policy.Permissions.Contains(permission);
        }

        /// <summary>
        /// Evaluates the policy for a given user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">user</exception>
        public Task<PolicyResult> EvaluateAsync(ClaimsPrincipal user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return this._businessApplications.EvaluateAsync(user, _permissionService);
        }
    }
}