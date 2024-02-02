using System;

namespace PolicyBased.Infra.Dtos
{
    /// <summary>
    /// Represents an instance of a Business Application
    /// </summary>
    public class Application
    {
        /// <summary>
        /// Represents an Application Identifier.
        /// </summary>
        public int Id { get; set; } = default!;

        /// <summary>
        /// The name of the application.
        /// </summary>
        public string Name { get; set; } = default!;

        /// <summary>
        /// Represents the policies associated to a Business Application.
        /// </summary>
        public List<Policy> Policies { get; set; }
        public int TenantId { get; set; } = default!;


        // TODO: remove this later or refactor to a better design...
        // /// <summary>
        // /// The roles of the application.
        // /// </summary>
        // public Role[] Roles { get; set; }

        // /// <summary>
        // /// The permissions associated to the Roles.
        // /// </summary>
        // public Permission[] Permissions { get; set; }
    }
}