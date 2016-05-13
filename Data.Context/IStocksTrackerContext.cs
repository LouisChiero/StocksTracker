using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Data.Context
{
    /// <summary>
    /// Interface provides a facade to properties and methods available to a database context.
    /// </summary>
    public interface IStocksTrackerContext : IDisposable
    {
        /// <summary>
        /// Gets the Stocks value.
        /// </summary>
        IDbSet<Stock> Stocks { get; }

        /// <summary>
        /// Gets the StockTrackers value.
        /// </summary>
        IDbSet<StockTracker> StockTrackers { get; }

        /// <summary>
        /// Gets the StockTrackerStocks value.
        /// </summary>
        IDbSet<StockTrackerStock> StockTrackerStocks { get; }
        
        /// <summary>
        /// Gets the Users value.
        /// </summary>
        IDbSet<ApplicationUser> Users { get; }

        /// <summary>
        /// Gets the Roles value.
        /// </summary>
        IDbSet<IdentityRole> Roles { get; }

        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>The number of objects affected by the save.</returns>
        int SaveChanges();

        /// <summary>
        /// Saves all changes made in this context to the underlying database asynchronously.
        /// </summary>
        /// <returns>The number of objects affected by the save.</returns>
        Task<int> SaveChangesAsync();
        
        /// <summary>
        /// Method will let a caller know if the context is configured correctly, and active.
        /// </summary>
        void Ping();
    }
}
