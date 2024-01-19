using System.Security.Claims;

namespace PolicyBased.Infra.Dtos
{
    /// <summary>
    /// Models a policy
    /// </summary>
    public class Policy
    {
        /// <summary>
        /// The Id of the Policy
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Name of the Policy
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        public List<Role> Roles { get; set; } = new List<Role>();

        /// <summary>
        /// Gets the permissions.
        /// </summary>
        /// <value>
        /// The permissions.
        /// </value>
        public List<Permission> Permissions { get; set; } = new List<Permission>();
       
    }
}