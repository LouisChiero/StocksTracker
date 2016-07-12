using System.Collections.Generic;

namespace Core.Framework.API.Data
{
    /// <summary>
    /// Interface implemented by classes that need to manage caches.
    /// </summary>
    public interface ICacheManager<T>
    {
        /// <summary>
        /// Method loads a data cache.
        /// </summary>
        void InitializeCache();

        void AddObjectToCache(T cacheObject);

        T GetCachedObject(int id);

        IEnumerable<T> GetAllCachedObjects();

        void UpdateCachedObject(T cachedObject);

        void ClearCachedObjects();

        bool ObjectExistsInCache(int id);
    }
}
