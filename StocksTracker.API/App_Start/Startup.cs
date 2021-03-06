﻿using System.Web;
using Autofac;
using Core.Framework.API.Data;
using Core.Framework.API.Stocks;
using Core.Framework.API.Support;
using Data.Context;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Owin.Security.Providers.GitHub;

namespace StocksTracker.API
{
    /// <summary>
    /// Class configures OWIN for the application.
    /// </summary>
    public partial class Startup
    {
        static Startup()
        {
            var iocContainer = (IContainer)HttpContext.Current.Items[WebApiApplication.ContainerIdentifier];
            OAuthOptions = iocContainer.Resolve<OAuthAuthorizationServerOptions>();
            GitHubOAuthOptions = iocContainer.Resolve<GitHubAuthenticationOptions>();
            StocksTrackerContextFactory = iocContainer.Resolve<IObjectFactory<StocksTrackerContext>>();
            StocksCacheManager = iocContainer.Resolve<ICacheManager<StockRecord>>();
        }

        /// <summary>
        /// Gets the open authorization options for the server.
        /// </summary>
        private static OAuthAuthorizationServerOptions OAuthOptions { get; set; }

        private static GitHubAuthenticationOptions GitHubOAuthOptions { get; set; }

        /// <summary>
        /// Gets a reference to a factory for creating stocks tracker contexts.
        /// </summary>
        private static IObjectFactory<StocksTrackerContext> StocksTrackerContextFactory { get; set; }

        /// <summary>
        /// Gets a reference to an ICacheManager object.
        /// </summary>
        private static ICacheManager<StockRecord> StocksCacheManager { get; set; }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // allow use of cross-origin requests
            // http://stackoverflow.com/questions/24461605/angularjs-and-owin-authentication-on-webapi
            app.UseCors(CorsOptions.AllowAll);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);

            //app.UseOAuthBearerAuthentication(null);
            /*
             * Take a look at OAuthBearerAuthenticationProvider and its ValidateIdentity method: from there, you have a full access to the validating context and the deserialized identity. 
             * This way, you can send back any error using context.SetError('invalid_grant', 'message') if the deserialized identity doesn't meet your security requirements. 
             * While you shouldn't repeat validation steps already done by Katana - integrity and freshness of the token - you can of course query your database to determine whether the access token or the authorization used to issue the token has expired or not. 
             */

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //    consumerKey: "",
            //    consumerSecret: "");

            //app.UseFacebookAuthentication(
            //    appId: "",
            //    appSecret: "");

            app.UseGoogleAuthentication("google", "google");
            app.UseGitHubAuthentication(GitHubOAuthOptions);
        }

        private static void PingDatabase()
        {
          using (var ctx = StocksTrackerContextFactory.GetObject())
          {
            ctx.Ping();
          }                   
        }

        private static void LoadCaches()
        {
            StocksCacheManager.InitializeCache();
        }
    }
}