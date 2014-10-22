using System.Security.Claims;
using System.Threading.Tasks;
using Core.Framework.API.Support;
using Data.Context;
using Data.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StocksTracker.API.Extensions;

namespace StocksTracker.API.UserAdministration
{
    /// <summary>
    /// Implementation of <see cref="IUserAdministration"/> interface, providing access to CRUD operations for users and identities.
    /// </summary>
    public class UserAdministration : IUserAdministration
    {
        private readonly IObjectFactory<StocksTrackerContext> _stocksTrackerContextFactory;

        /// <summary>
        /// Instantiates the UserAdministration class.
        /// </summary>
        /// <param name="stocksTrackerContextFactory">Reference to a factory that returns stocks tracker contexts.</param>
        public UserAdministration(IObjectFactory<StocksTrackerContext> stocksTrackerContextFactory)
        {
            _stocksTrackerContextFactory = stocksTrackerContextFactory;
        }

        /// <see cref="IUserAdministration.GetUserAsync(string)"/>
        public async Task<User> GetUserAsync(string userId)
        {
            using (var ctx = _stocksTrackerContextFactory.GetObject())
            {
                using (var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ctx)))
                {
                    var user = await userManager.FindByIdAsync(userId);
                    if(user != null)
                        return user.MapUserRecord();
                }
            }

            return null;
        }

        /// <see cref="IUserAdministration.GetUserAsync(string, string)"/>
        public async Task<User> GetUserAsync(string userName, string password)
        {
            using (var ctx = _stocksTrackerContextFactory.GetObject())
            {
                using (var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ctx)))
                {
                    var user = await userManager.FindAsync(userName, password);
                    if (user != null)
                        return user.MapUserRecord();
                }
            }

            return null;
        }

        /// <see cref="IUserAdministration.CreateUserAsync"/>
        public async Task<IdentityResult> CreateUserAsync(string userName, string password)
        {
            using (var ctx = _stocksTrackerContextFactory.GetObject())
            {
                using (var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ctx)))
                {
                    return await userManager.CreateAsync(new ApplicationUser { UserName = userName }, password);
                }
            }
        }

        /// <see cref="IUserAdministration.CreateIdentityAsync"/>
        public async Task<ClaimsIdentity> CreateIdentityAsync(User user, string authenticationType)
        {
            using (var ctx = _stocksTrackerContextFactory.GetObject())
            {
                using (var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ctx)))
                {
                    var appUser = await userManager.FindByIdAsync(user.Identity.Id);
                    if (appUser == null)
                        return null;

                    return await userManager.CreateIdentityAsync(appUser, authenticationType);
                }
            }
        }

        /// <see cref="IUserAdministration.RoleExists"/>
        public bool RoleExists(string roleName)
        {
            using (var ctx = _stocksTrackerContextFactory.GetObject())
            {
                using (var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(ctx)))
                {
                    return roleManager.RoleExists(roleName);
                }
            }
        }

        /// <see cref="IUserAdministration.CreateRole"/>
        public IdentityResult CreateRole(string roleName)
        {
            using (var ctx = _stocksTrackerContextFactory.GetObject())
            {
                using (var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(ctx)))
                {
                    return roleManager.Create(new IdentityRole(roleName));
                }
            }
        }

        /// <see cref="IUserAdministration.FindByUserName"/>
        public User FindByUserName(string userName)
        {
            using (var ctx = _stocksTrackerContextFactory.GetObject())
            {
                using (var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ctx)))
                {
                    var user = userManager.FindByName(userName);
                    if (user != null)
                        return user.MapUserRecord();
                }
            }

            return null;
        }

        /// <see cref="IUserAdministration.CreateUser"/>
        public IdentityResult CreateUser(User user, string password)
        {
            using (var ctx = _stocksTrackerContextFactory.GetObject())
            {
                using (var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ctx)))
                {
                    var newUser = new ApplicationUser
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Identity.Email,
                        UserName = user.Identity.UserName
                    };

                    return userManager.Create(newUser, password);
                }
            }
        }

        /// <see cref="IUserAdministration.CreateUser"/>
        public IdentityResult AddRoleToUser(string userId, string roleName)
        {
            using (var ctx = _stocksTrackerContextFactory.GetObject())
            {
                using (var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ctx)))
                {
                    return userManager.AddToRole(userId, roleName);
                }
            }
        }
    }
}