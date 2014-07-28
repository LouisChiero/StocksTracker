using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Data.Models
{
    /// <summary>
    /// Encapsulates the properties of an application user object.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Instantiates the ApplicationUser class.
        /// </summary>
        public ApplicationUser()
        {
            StockTrackers = new HashSet<StockTracker>();
        }

        /// <summary>
        /// Gets and sets the first name value.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets and sets the last name value.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets and sets a collection of this application user's <see cref="StockTracker"/> objects.
        /// </summary>
        public ICollection<StockTracker> StockTrackers { get; set; }
    }
}
