namespace Core.Framework.API.Data
{
    /// <summary>
    /// Interface implemented by classes that need to manage caches.
    /// </summary>
    public interface ICacheManager
    {
        /// <summary>
        /// Method loads a data cache.
        /// </summary>
        void LoadDataCache();
    }
}
