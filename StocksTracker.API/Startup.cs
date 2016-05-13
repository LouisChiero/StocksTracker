using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(StocksTracker.API.Startup))]

namespace StocksTracker.API
{
    /// <summary>
    /// Class called on startup to configure OWIN authentication values.
    /// </summary>
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            ConfigureAuth(app);
            PingDatabase();
            LoadCaches();
        }
    }
}
