using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using StocksTracker.API.UserAdministration;

namespace StocksTracker.API.Authorization
{
    /// <summary>
    /// Class provides communication between the middleware authorization server and the application.
    /// </summary>
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;
        private readonly IUserAdministration _userAdministration;

        /// <summary>
        /// Instantiates the ApplicationOAuthProvider class.
        /// </summary>
        /// <param name="publicClientId">A value used to identify the client application.</param>
        /// <param name="userAdministration">Reference to an object that provides user administration access.</param>
        public ApplicationOAuthProvider(
            string publicClientId,
            IUserAdministration userAdministration)
        {
            _publicClientId = publicClientId;
            _userAdministration = userAdministration;
        }

        /// <see cref="OAuthAuthorizationServerProvider.GrantResourceOwnerCredentials"/>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var user = await _userAdministration.GetUserAsync(context.UserName, context.Password);
            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            ClaimsIdentity oAuthIdentity =
                await _userAdministration.CreateIdentityAsync(user, context.Options.AuthenticationType);
            ClaimsIdentity cookiesIdentity =
                await _userAdministration.CreateIdentityAsync(user, CookieAuthenticationDefaults.AuthenticationType);

            AuthenticationProperties properties = CreateProperties(user.Identity.UserName);
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }

        /// <see cref="OAuthAuthorizationServerProvider.GrantResourceOwnerCredentials"/>
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        /// <see cref="OAuthAuthorizationServerProvider.GrantResourceOwnerCredentials"/>
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        /// <see cref="OAuthAuthorizationServerProvider.ValidateClientRedirectUri"/>
        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }
        
        /// <see cref="OAuthAuthorizationServerProvider.GrantClientCredentials"/>
        public override Task GrantClientCredentials(OAuthGrantClientCredentialsContext context)
        {
            return base.GrantClientCredentials(context);
        }

        /// <see cref="OAuthAuthorizationServerProvider.AuthorizeEndpoint"/>
        public override Task AuthorizeEndpoint(OAuthAuthorizeEndpointContext context)
        {
            return base.AuthorizeEndpoint(context);
        }

        /// <see cref="OAuthAuthorizationServerProvider.GrantAuthorizationCode"/>
        public override Task GrantAuthorizationCode(OAuthGrantAuthorizationCodeContext context)
        {
            return base.GrantAuthorizationCode(context);
        }

        /// <see cref="OAuthAuthorizationServerProvider.ValidateAuthorizeRequest"/>
        public override Task ValidateAuthorizeRequest(OAuthValidateAuthorizeRequestContext context)
        {
            return base.ValidateAuthorizeRequest(context);
        }

        /// <see cref="OAuthAuthorizationServerProvider.ValidateTokenRequest"/>
        public override Task ValidateTokenRequest(OAuthValidateTokenRequestContext context)
        {
            return base.ValidateTokenRequest(context);
        }

        private static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }

    }
}