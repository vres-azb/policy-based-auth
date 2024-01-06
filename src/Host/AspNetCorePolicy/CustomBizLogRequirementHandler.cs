using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using PolicyServer.Runtime.Client;

namespace Host.AspNetCorePolicy
{
    public class CustomBizLogRequirementHandler : AuthorizationHandler<CustomBizLogRequirement>
    {
        private readonly IPolicyServerRuntimeClient _client;

        public CustomBizLogRequirementHandler(IPolicyServerRuntimeClient client)
        {
            _client = client;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomBizLogRequirement requirement)
        {
            var user = context.User; var allowed = false;

            if (await _client.HasPermissionAsync(user, "PerformCustomBizLogRequirement"))
            {
                if (requirement.ConditionAmount <= 10) allowed = true;
                else allowed = await _client.IsInRoleAsync(user, "Manager");

                if (allowed || requirement.ConditionName == "approve")
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}