using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolicyServer.Runtime.Client;

namespace Host.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IPolicyServerRuntimeClient _client;
        private readonly IAuthorizationService _authz;

        public OrdersController(IPolicyServerRuntimeClient client, IAuthorizationService authz)
        {
            _client = client;
            _authz = authz;
        }

        //[Authorize("MANAGE")]
        public async Task<IActionResult> Create()
        {
            var isUserAllowed = await this._authz.AuthorizeAsync(User, "MANAGE");
            var policyResult = await this._client.EvaluateAsync(User);

            // Q: Has the logged in user permission based on the app-soecific roles?
            var hasPermissionToExportOrderToPDF = await _client.HasPermissionAsync(User, "CanExportOrderToPDF");

            // Q: What is the flow when...
            // a) You don't use the Authorize[] attribute
            // b) You do use the Authorize[] attribute
            //var isUserAllowed = await this._authz.AuthorizeAsync(User, "CanExportOrderToPDF");
            if (!isUserAllowed.Succeeded)
            {
                return Forbid();
            }

            return View("success");
        }

        [Authorize("VIEW")]
        public async Task<IActionResult> Read()
        {
            //var isUserAllowed = await this._authz.AuthorizeAsync(User, "VIEW");
            return View("success");
        }

        [Authorize("MANAGE")]
        public IActionResult Modify()
        {
            return View("success");
        }


        [Authorize(Policy = "ADMIN")]
        public IActionResult Delete()
        {
            return View("success");
        }

    }
}