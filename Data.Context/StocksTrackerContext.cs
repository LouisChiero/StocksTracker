using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Data.Context.Configuration;
using Data.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Data.Context
{
    /// <summary>
    /// Class provides access to Entity Framework CRUD actions within the Stocks Tracker domain.
    /// </summary>
    public class StocksTrackerContext : IdentityDbContext<ApplicationUser>, IStocksTrackerContext
    {
        private readonly bool _eagerOpen;

        private IDbSet<Stock> _stocks;
        private IDbSet<StockTracker> _stockTrackers;
        private IDbSet<StockTrackerStock> _stockTrackerStocks;

        /// <summary>
        /// This constructor forces Entity Framework to search for and use a connection string
        /// of the same name (if it exists).
        /// <remarks>
        /// For EF Code First, the database will be created if it does not exist.
        /// </remarks>
        /// </summary>
        public StocksTrackerContext()
            : base("name=StocksTrackerContext")
        {
        }

        /// <summary>
        /// This constructor will override the default Entity Framework naming convention for
        /// the database.
        /// <remarks>
        /// For EF Code First, the database will be created if it does not exist.
        /// </remarks>
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        public StocksTrackerContext(string databaseName)
            :base(databaseName)
        {
        }

        /// <summary>
        /// This constructor forces Entity Framework connect to the database using the passed in
        /// connection string.
        /// <remarks>
        /// Will not create the database if it does not exist.  Strictly for opening a connection to 
        /// an existing database.
        /// </remarks>
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="eagerOpen">Whether to automatically open a connection to the database.</param>
        public StocksTrackerContext(string connectionString, bool eagerOpen)
            : base(connectionString)
        {
            _eagerOpen = eagerOpen;
            if (eagerOpen)
                Database.Connection.Open();
        }

        /// <see cref="IStocksTrackerContext.Stocks"/>
        public IDbSet<Stock> Stocks
        {
            get { return _stocks ?? (_stocks = Set<Stock>()); }
        }

        /// <see cref="IStocksTrackerContext.StockTrackers"/>
        public IDbSet<StockTracker> StockTrackers
        {
            get { return _stockTrackers ?? (_stockTrackers = Set<StockTracker>()); }
        }

        /// <see cref="IStocksTrackerContext.StockTrackerStocks"/>
        public IDbSet<StockTrackerStock> StockTrackerStocks
        {
            get { return _stockTrackerStocks ?? (_stockTrackerStocks = Set<StockTrackerStock>()); }
        }

        /// <see cref="IStocksTrackerContext.SaveChanges"/>
        public new int SaveChanges()
        {
            return base.SaveChanges();
        }        

        /// <see cref="IStocksTrackerContext.SaveChangesAsync"/>
        public new Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        /// <see cref="IStocksTrackerContext.Ping"/>
        public void Ping()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Stocks.Any();
        }

        /// <see cref="DbContext.OnModelCreating"/>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new StockConfiguration());
            modelBuilder.Configurations.Add(new StockTrackerConfiguration());
            modelBuilder.Configurations.Add(new StockTrackerStockConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        #region IDisposable members

        /// <see cref="IDisposable.Dispose"/>
        public new void Dispose(bool disposing)
        {
            if (disposing && _eagerOpen)
                Database.Connection.Close();

            base.Dispose(disposing);
        }

        #endregion
    }
}
