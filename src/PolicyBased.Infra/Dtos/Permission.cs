using System;
using System.Collections.Generic;
using System.Linq;

namespace PolicyBased.Infra.Dtos
{
    /// <summary>
    /// Models a permission
    /// </summary>
    public class Permission
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        public bool IsSelected { get; set; } = default!;
        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        public List<string> Roles { get; set; } = new List<string>();

        // TODO: this MUST be internal scope, making this public for demo
        public bool Evaluate(IEnumerable<string> roles)
        {
            if (roles == null) throw new ArgumentNullException(nameof(roles));

            if (Roles.Any(x => roles.Contains(x))) return true;

            return false;
        }
    }
}