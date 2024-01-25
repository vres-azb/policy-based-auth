using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FakeRepos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolicyBased.Contracts;
using PolicyBased.Contracts.Services;

namespace Host.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IFakeRepository _fakeRepository;
        private readonly IPermissionService _permissionService;

        public AccountController(IFakeRepository fakeRepository, IPermissionService permissionService)
        {
            this._fakeRepository = fakeRepository;
            this._permissionService = permissionService;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string userName, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!string.IsNullOrWhiteSpace(userName))
            {
                //var claimsPrincipal = this._fakeRepository.RetrieveUser(userName);
                var claimsPrincipal = await this.BuildClaimsPrincipalAsync(userName);

                await HttpContext.SignInAsync(claimsPrincipal);
                return LocalRedirect(returnUrl);
            }

            return View();
        }

        // TODO: Bad! refactor this later...
        private async Task<ClaimsPrincipal> BuildClaimsPrincipalAsync(string userName)
        {
            LoggedInUser loggedInUser = await this._permissionService.GetLoggedInUserAsync(userName);

            List<Claim> userClaims = new List<Claim>();
            userClaims.AddRange(new[]
            {
                new Claim("sub",loggedInUser.Sub),
                new Claim("name",loggedInUser.Name),
                new Claim("tenantId",loggedInUser.TenantId.ToString()),
            });

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(userClaims, "password", "name", "role");
            ClaimsPrincipal result = new ClaimsPrincipal(claimsIdentity);
            return result;
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        public IActionResult AccessDenied() => View();
    }
}