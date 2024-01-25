namespace PolicyBased.Contracts.Services;

public interface IPermissionService
{
    Task<PolicyResult> GetPermissions(string userId);
    Task<LoggedInUser> GetLoggedInUserAsync(string userName);
}
