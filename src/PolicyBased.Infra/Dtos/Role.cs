using System.Security.Claims;

namespace PolicyBased.Infra.Dtos
{
    /// <summary>
    /// Models an application role
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        public bool IsSelected { get; set; } = default!;

        public List<string> Permissions { get; set; } = default!;     //KK
        /// <summary>
        /// Gets the subjects.
        /// </summary>
        /// <value>
        /// The subjects.
        /// </value>
        public List<string> Subjects { get; set; } = new List<string>();

        /// <summary>
        /// Gets the identity roles.
        /// </summary>
        /// <value>
        /// The identity roles.
        /// </value>
        public List<string> IdentityRoles { get; set; } = new List<string>();

        // TODO: this MUST be internal scope, making this public for demo
        public bool Evaluate(ClaimsPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var sub = user.FindFirst("sub")?.Value;
            if (!String.IsNullOrWhiteSpace(sub))
            {
                if (Subjects.Contains(sub)) return true;
            }

            var roles = user.FindAll("role").Select(x => x.Value);
            if (roles.Any())
            {
                if (IdentityRoles.Any(x => roles.Contains(x))) return true;
            }

            return false;
        }
    }
}