using Microsoft.AspNetCore.Authorization;

namespace Host.AspNetCorePolicy
{
    public class CustomBizLogRequirement : IAuthorizationRequirement
    {
        public string ConditionName { get; set; }
        public int ConditionAmount { get; set; }
    }
}