using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Framework.API.Stocks
{
    /// <summary>
    /// Interface defines actions that can be performed on stock tracker objects.
    /// </summary>
    public interface IStockTrackers
    {
        /// <summary>
        /// Asynchronously queries a stock tracker for the given ID.
        /// </summary>
        /// <param name="stockTrackerId">The stock tracker ID.</param>
        /// <returns>A <see cref="StockTrackerRecord"/> object, or null if not found.</returns>
        Task<StockTrackerRecord> GetStockTrackerAsync(int stockTrackerId);

        /// <summary>
        /// Asynchronously queries all stock trackers for a user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>A collection of <see cref="StockTrackerRecord"/> objects for the user.</returns>
        Task<IEnumerable<StockTrackerRecord>> GetStockTrackersForUser(string userId);

        /// <summary>
        /// Asynchronously adds a stock tracker for a user.
        /// </summary>
        /// <param name="addStockTracker">Properties of the new stock tracker.</param>
        /// <param name="userId">The user ID.</param>
        /// <returns>The newly added <see cref="StockTrackerRecord"/> object.</returns>
        Task<StockTrackerRecord> AddStockTrackerForUserAsync(StockTrackerRecord addStockTracker, string userId);

        /// <summary>
        /// Asynchronously removes a stock tracker for the given ID.
        /// </summary>
        /// <param name="stockTrackerId">The stock tracker ID></param>
        /// <returns>The result of the removal action.</returns>
        Task<StockTrackerOperationResult> RemoveStockTrackerAsync(int stockTrackerId);

        /// <summary>
        /// Asynchronously updates a stock tracer.
        /// </summary>
        /// <param name="updateStockTracker">Properties of the updated stock tracker.</param>
        /// <returns>An updated <see cref="StockTrackerRecord"/> object.</returns>
        Task<StockTrackerRecord> UpdateStockTrackerAsync(StockTrackerRecord updateStockTracker);
    }
}
