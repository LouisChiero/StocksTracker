using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using System.Web;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Core.Framework.API.Charts;
using Core.Framework.API.Data;
using Core.Framework.API.Headlines;
using Core.Framework.API.Quotes;
using Core.Framework.API.Stocks;
using Core.Framework.API.Support;
using Data.Context;
using Data.Context.Support;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using StocksTracker.API.Authorization;
using StocksTracker.API.Cache;
using StocksTracker.API.Services;
using StocksTracker.API.UserAdministration;

namespace StocksTracker.API
{
    public class WebApiApplication : HttpApplication
    {
        private IContainer _container;
        private const string IocContainer = "IOC_CONTAINER";
        public static string ContainerIdentifier { get { return IocContainer; } }

        protected void Application_Start()
        {
            // create IoC container
            var builder = new ContainerBuilder();

            // register all API controllers
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // register stock tracker context as singleton
            var connectionTimeout = ConfigurationManager.AppSettings["Database.Timeout"];
            int timeout = string.IsNullOrWhiteSpace(connectionTimeout) ? 3600 : Convert.ToInt32(connectionTimeout);
            var sqlConnectionBuilder = new SqlConnectionStringBuilder
            {
                DataSource = ConfigurationManager.AppSettings["Database.Server"],
                InitialCatalog = ConfigurationManager.AppSettings["Database.Name"],
                MultipleActiveResultSets = true,
                IntegratedSecurity = true,
                ConnectTimeout = timeout
            };

            builder.Register(pcf => new StocksTrackerContextFactory(sqlConnectionBuilder, false))
                .As<IObjectFactory<StocksTrackerContext>>()
                .SingleInstance();

            // register core business interfaces
            builder.RegisterType<StockCache>().As<ICache<StockRecord>>().SingleInstance();
            builder.RegisterType<StocksService>().As<IStocks>().SingleInstance();
            builder.RegisterType<StockCacheManager>().As<ICacheManager>().SingleInstance();
            builder.RegisterType<StockTrackersService>().As<IStockTrackers>().SingleInstance();
            builder.RegisterType<StockQuoteService>().As<IStockQuoteService>().SingleInstance();
            builder.RegisterType<StockUpdater>().As<IStockUpdater>().SingleInstance();
            builder.RegisterType<StockHeadlinesService>().As<IStockHeadlinesService>().SingleInstance();
            builder.RegisterType<StockChartsService>().As<IStockChartsService>().SingleInstance();

            // register objects used in user authentication/authorization
            builder.RegisterType<UserAdministration.UserAdministration>().As<IUserAdministration>().SingleInstance();
            builder.Register(provider => new ApplicationOAuthProvider(
                ConfigurationManager.AppSettings["Identity.PublicClientId"],
                _container.Resolve<IUserAdministration>()))
                .As<IOAuthAuthorizationServerProvider>()
                .SingleInstance();

            // register object that communication between application and middleware authorization server
            var tokenExpireDays = ConfigurationManager.AppSettings["Identity.TokenExpireDays"];
            int days = string.IsNullOrWhiteSpace(tokenExpireDays) ? 14 : Convert.ToInt32(tokenExpireDays);
            builder.Register(options => new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString(ConfigurationManager.AppSettings["Identity.TokenEndpointPath"]),
                AuthorizeEndpointPath = new PathString(ConfigurationManager.AppSettings["Identity.AuthorizeEndpointPath"]),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(days),
                AllowInsecureHttp = true,
                Provider = _container.Resolve<IOAuthAuthorizationServerProvider>()
            })
            .As<OAuthAuthorizationServerOptions>()
            .SingleInstance();

            // build container, and set dependency resolver
            _container = builder.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(_container);

            // configure API
            GlobalConfiguration.Configure(WebApiConfig.Register);

            HttpContext.Current.Items[IocContainer] = _container;
        }

        protected void Application_End()
        {
            if (HttpContext.Current != null && HttpContext.Current.Items[IocContainer] != null)
                ((IContainer)HttpContext.Current.Items[IocContainer]).Dispose();
        }
    }
}
