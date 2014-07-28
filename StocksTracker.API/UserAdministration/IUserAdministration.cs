using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace StocksTracker.API.UserAdministration
{
    /// <summary>
    /// Interface defines CRUD methods available on application users.
    /// </summary>
    public interface IUserAdministration
    {
        /// <summary>
        /// Asynchronously gets a user for the given ID, or null if the user is not found.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A user object, or null if not found.</returns>
        Task<User> GetUserAsync(string userId);

        /// <summary>
        /// Asynchronously gets a user for the given user name and password.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="password">The user password.</param>
        /// <returns>A user object, or null if not found.</returns>
        Task<User> GetUserAsync(string userName, string password);

        /// <summary>
        /// Asynchronously creates a user for the given user name and password.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="password">The user password.</param>
        /// <returns>The result of the operation.</returns>
        Task<IdentityResult> CreateUserAsync(string userName, string password);

        /// <summary>
        /// Asynchronously creates an identity for a given user.
        /// </summary>
        /// <param name="user">The user for whom a claims identity will be created.</param>
        /// <param name="authenticationType">The type of authentication for the new claims identity.</param>
        /// <returns>The new claims identity.</returns>
        Task<ClaimsIdentity> CreateIdentityAsync(User user, string authenticationType);
    }
}
