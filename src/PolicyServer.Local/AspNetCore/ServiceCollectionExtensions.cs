using FakeRepos;
using PolicyServer.Runtime.Client;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Helper class to configure DI
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the policy server client.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static PolicyServerBuilder AddPolicyServerClient(this IServiceCollection services)
        {
            //services.Configure<Policy>(configuration);
            services.AddSingleton<IFakeRepository, FakeRepository>();


            services.AddTransient<IPolicyServerRuntimeClient, PolicyServerRuntimeClient>();
            services.AddScoped(provider => provider.GetRequiredService<IFakeRepository>().RetrieveBusinessApps());


            return new PolicyServerBuilder(services);
        }
    }
}