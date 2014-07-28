using System.Collections.Generic;
using System.Linq;

namespace StocksTracker.API.Extensions
{
    /// <summary>
    /// Class provides extension methods that augment the <see cref="IEnumerable{T}"/> interface.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Generates an IEnumerable from a single value.
        /// </summary>
        /// <param name="value">The value to create an enumerable for</param>
        /// <returns>An enumerable for the single value</returns>
        public static IEnumerable<T> ToEnumerable<T>(this T value)
        {
            yield return value;
        }

        /// <summary>
        /// Determines whether an enumerable contains any items. This method
        /// contains an optimization for Collections/Lists using the Count property,
        /// but for other enumerables Any() is used, which is more efficient than
        /// checking the Count() method.
        /// </summary>
        /// <param name="items">The enumerable to check for items</param>
        /// <returns>Whether the enumerable contains at least one item</returns>
        public static bool IsEmpty<T>(this IEnumerable<T> items)
        {
            // Optimize for collections since the Count property is more efficient.
            var itemsCollection = items as ICollection<T>;
            if (itemsCollection != null)
                return itemsCollection.Count == 0;

            return !items.Any();
        }
    }
}