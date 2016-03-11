using CandidateManager.Web.Initializers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CandidateManager.Web.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser
    // class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class CandidateManagerUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<CandidateManagerUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class CandidateManagerIdentityContext : IdentityDbContext<CandidateManagerUser>
    {
        public CandidateManagerIdentityContext()
            : base("CandidateManagerIdentityConString", false)
        {
            Database.SetInitializer(new CandidateManagerInitializer());
        }

        public static CandidateManagerIdentityContext Create()
        {
            return new CandidateManagerIdentityContext();
        }
    }
}
