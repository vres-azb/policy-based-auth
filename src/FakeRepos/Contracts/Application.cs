using System;

namespace FakeRepos
{
    /// <summary>
    /// Represents an instance of a Business Application
    /// </summary>
    public class Application
    {
        /// <summary>
        /// Represents an Application Identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the application.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Represents the policies associated to a Business Application.
        /// </summary>
        public Policy[] Policies { get; set; }


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