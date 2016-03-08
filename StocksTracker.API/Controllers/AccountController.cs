using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using StocksTracker.API.Models;
using StocksTracker.API.UserAdministration;

namespace StocksTracker.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private readonly IUserAdministration _userAdministration;

        public AccountController(IUserAdministration userAdministration)
        {
            _userAdministration = userAdministration;
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterUserViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            IdentityResult result = await _userAdministration.CreateUserAsync(model.UserName, model.Password);
            IHttpActionResult errorResult = GetErrorResult(result);

            return errorResult ?? Ok();
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // GET: api/Account/ExternalLogin/?provider=Google
        [HttpGet]
        [AllowAnonymous]
        [Route("ExternalLogin")]
        public async Task<IHttpActionResult> ExternalLogin(string provider)//, string returnUrl)
        {
            // Request a redirect to the external login provider
            return await Task.Run(() => new ChallengeResult(provider, this));//Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ChallengeResult : IHttpActionResult
        {
            public ChallengeResult(string loginProvider, ApiController controller)
            {
                LoginProvider = loginProvider;
                Request = controller.Request;
            }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                Request.GetOwinContext().Authentication.Challenge(LoginProvider);

                var response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    RequestMessage = Request
                };

                return Task.FromResult(response);
            }

            private string LoginProvider { get; set; }
            private HttpRequestMessage Request { get; set; }
        }
    }
}
