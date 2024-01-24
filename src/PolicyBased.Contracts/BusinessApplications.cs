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

            // TODO: This may not be needed as we are getting the daa from DB
            Application app = Applications.FirstOrDefault(x => x.Id == 1);
            Policy policy = app.Policies.FirstOrDefault(x => x.Id == 1);


            var roles = policy.Roles.Where(x => x.Evaluate(user)).Select(x => x.Name).ToArray();
            var permissions = policy.Permissions.Where(x => x.Evaluate(roles)).Select(x => x.Name).ToArray();

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