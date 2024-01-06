﻿using PolicyServer.Runtime.Client.AspNetCore;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// PolicyServer extensions for IApplicationBuilder
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Add the policy server claims transformation middleware to the pipeline.
        /// This middleware will turn application roles and permissions into claims and add them to the current user
        /// </summary>
        /// <param name="app">The application.</param>
        /// <returns></returns>
        public static IApplicationBuilder UsePolicyServerClaims(this IApplicationBuilder app)
        {
            return app.UseMiddleware<PolicyServerClaimsMiddleware>();
        }
    }
}