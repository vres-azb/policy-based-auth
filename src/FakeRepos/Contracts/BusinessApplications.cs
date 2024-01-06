using System.Data;
using System.Security.Claims;

namespace FakeRepos
{
    /// <summary>
    /// The Business Applications
    /// </summary>
    public class BusinessApplications
    {

        public int Id { get; set; }
        
        /// <summary>
        /// The Busines Applications collection
        /// </summary>
        public IEnumerable<Application> Applications { get; set; }

        public Task<PolicyResult> EvaluateAsync(ClaimsPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            // TODO: refactor this later...
            Application app = this.Applications.FirstOrDefault(x=>x.Id == 1);
            Policy policy = app.Policies.FirstOrDefault(x=>x.Id == 1);


            var roles = policy.Roles.Where(x => x.Evaluate(user)).Select(x => x.Name).ToArray();
            var permissions = policy.Permissions.Where(x => x.Evaluate(roles)).Select(x => x.Name).ToArray();

            var result = new PolicyResult()
            {
                Roles = roles.Distinct(),
                Permissions = permissions.Distinct()
            };

            return Task.FromResult(result);
        }
    }
}