using System.Collections.Generic;

namespace PolicyBased.Contracts
{
    /// <summary>
    /// The result of a policy evaluation
    /// </summary>
    public class PolicyResult
    {
        /// <summary>
        /// The name of the policy
        /// </summary>
        public IEnumerable<string> Policies { get; set; }

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        public IEnumerable<string> Roles { get; set; }

        /// <summary>
        /// Gets the permissions.
        /// </summary>
        /// <value>
        /// The permissions.
        /// </value>
        public IEnumerable<string> Permissions { get; set; }
    }
}
