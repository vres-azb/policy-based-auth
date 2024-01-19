using AuthAdminUI.Areas.Identity;
using AuthAdminUI.Areas.Identity.Data;
using AuthAdminUI.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Fast.Components.FluentUI;
using PolicyBased.Infra.Persistence.Context;
using PolicyBased.Infra.Persistence.Repositories;

namespace AuthAdminUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("AuthAdminUIContextConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<AuthAdminUIContext>(options =>
                options.UseSqlite(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddDefaultIdentity<AuthAdminUIUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<AuthAdminUIContext>();
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<AuthAdminUIUser>>();
            builder.Services.AddSingleton<WeatherForecastService>();
            builder.Services.AddFluentUIComponents();
            var services = builder.Services;
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IDataContextFactory, DataContextFactory>();
            services.AddScoped(provider =>
            {
                var factory = provider.GetRequiredService<IDataContextFactory>();
                return factory.CreateContext();
            });

            services.AddScoped<IUserRepo,UserRepo>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}