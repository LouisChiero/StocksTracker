using Microsoft.AspNet.Identity.EntityFramework;

namespace StocksTracker.API.UserAdministration
{
    /// <summary>
    /// Class encapsulates properties that make up a user.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets and sets the first name value.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets and sets the last name value.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets and sets the <see cref="IdentityUser"/> value.
        /// </summary>
        public IdentityUser Identity { get; set; }
    }
}