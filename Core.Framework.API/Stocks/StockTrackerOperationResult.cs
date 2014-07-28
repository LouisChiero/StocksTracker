namespace Core.Framework.API.Stocks
{
    /// <summary>
    /// Enumeration defines the results of stock tracker operations.
    /// </summary>
    public enum StockTrackerOperationResult
    {
        CreateSucceeded = 0,
        RemoveSucceeded = 1,
        UpdateSucceeded = 2,
        CreateFailed = 3,
        RemoveFailed = 4,
        UpdateFailed = 5
    }
}
