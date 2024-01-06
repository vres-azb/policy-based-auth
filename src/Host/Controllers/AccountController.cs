using System.Threading.Tasks;
using FakeRepos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IFakeRepository _fakeRepository;

        public AccountController(IFakeRepository fakeRepository)
        {
            this._fakeRepository = fakeRepository;
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
                var claimsPrincipal = this._fakeRepository.RetrieveUser(userName);

                await HttpContext.SignInAsync(claimsPrincipal);
                return LocalRedirect(returnUrl);
            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        public IActionResult AccessDenied() => View();
    }
}