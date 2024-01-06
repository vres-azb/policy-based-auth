﻿using System.Collections.Generic;
using Host.AspNetCorePolicy;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Host
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // `AddControllersWithViews()` Calls AddAuthorization under the hood.
            // Below, in the call to `UseEndpoints(...)` we require authorization to all controllers (besides controllers/actions that have `[AllowAnonymous]`).
            services.AddControllersWithViews();

            // this sets up authentication - for this demo we simply use a local cookie
            // typically authentication would be done using an external provider
            services
                .AddAuthentication("abes-sso-cookie")
                .AddCookie("abes-sso-cookie"); // TODO: make cookie secure per https://datatracker.ietf.org/doc/html/draft-ietf-httpbis-rfc6265bis#name-cookie-name-prefixes

            // this sets up the PolicyServer client library and policy provider - configuration is loaded from appsettings.json
            // services.AddPolicyServerClient(Configuration.GetSection("Policy"))
            services.AddPolicyServerClient()
            .AddAuthorizationPermissionPolicies();

            // this adds the necessary handler for our custom medication requirement
            services.AddTransient<IAuthorizationHandler, CustomBizLogRequirementHandler>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();

            // add this middleware to make roles and permissions available as claims
            // this is mainly useful for using the classic [Authorize(Roles="foo")] and IsInRole functionality
            // this is not needed if you use the client library directly or the new policy-based authorization framework in ASP.NET Core
            app.UsePolicyServerClaims();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute().RequireAuthorization();
            });
        }
    }
}