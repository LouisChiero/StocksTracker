using System.Collections.Generic;

namespace Core.Framework.API.Data
{
    /// <summary>
    /// Interface defines methods for generically caching objects.
    /// </summary>
    /// <typeparam name="T">The type of object being cached.</typeparam>
    public interface ICache<T>
    {
        /// <summary>
        /// Adds an object to the cache.
        /// </summary>
        /// <param name="cachedObject">The object being cached.</param>
        void AddToCache(T cachedObject);

        /// <summary>
        /// Gets a cached object by its unique ID.
        /// </summary>
        /// <param name="id">The object's unique ID.</param>
        /// <returns>A cached object, or null if not found.</returns>
        T GetById(int id);

        /// <summary>
        /// Gets all objects in the cache.
        /// </summary>
        /// <returns>All cached objects.</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Updates an object in the cache.
        /// </summary>
        /// <param name="updatedObject">An object that replaces a cached object.</param>
        void UpdateCachedObject(T updatedObject);

        /// <summary>
        /// Removes all objects from the cache.
        /// </summary>
        void Clear();
    }
}
