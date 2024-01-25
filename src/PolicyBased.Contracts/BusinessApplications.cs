using PolicyBased.Contracts.Services;
using System.Data;
using System.Security.Claims;

namespace PolicyBased.Contracts
{
    /// <summary>
    /// The Business Applications
    /// </summary>
    public class BusinessApplications
    {
        public int Id { get; set; } = default!;

        /// <summary>
        /// The Busines Applications collection
        /// </summary>
        public IEnumerable<Application> Applications { get; set; } = default!;

        // TODO: this MUST be internal scope, making this public for demo
        public async Task<PolicyResult> EvaluateAsync(ClaimsPrincipal user, IPermissionService permissionService)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var polResult = new PolicyResult();

            ClaimsPrincipal currentUser = user;
            var userVal = currentUser?.FindFirst("sub")?.Value;
            if (!string.IsNullOrEmpty(userVal))
            {
                polResult = await permissionService.GetPermissions(userVal);
            }

            return await Task.FromResult(polResult);
        }
    }
}