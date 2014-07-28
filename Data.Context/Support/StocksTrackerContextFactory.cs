using System.Data.SqlClient;
using Core.Framework.API.Support;

namespace Data.Context.Support
{
    /// <summary>
    /// Factory class creates and returns <see cref="StocksTrackerContext"/> database context classes.
    /// </summary>
    public class StocksTrackerContextFactory : IObjectFactory<StocksTrackerContext>
    {
        private readonly string _connectionString;
        private readonly bool _eagerOpen;

        /// <summary>
        /// Initializes the StocksTrackerContextFactory class.
        /// </summary>
        /// <param name="connectionStringBuilder">An object used to build SQL connection strings.</param>
        /// <param name="eagerOpen">Whether to explicitly open a connection to the context.</param>
        public StocksTrackerContextFactory(SqlConnectionStringBuilder connectionStringBuilder, bool eagerOpen)
        {
            _connectionString = connectionStringBuilder.ToString();
            _eagerOpen = eagerOpen;
        }

        /// <see cref="IObjectFactory{T}.GetObject()"/>
        public StocksTrackerContext GetObject()
        {
            return new StocksTrackerContext(_connectionString, _eagerOpen);
        }
    }
}
