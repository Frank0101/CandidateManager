using CandidateManager.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using WebGrease.Css.Extensions;

namespace CandidateManager.Web.Initializers
{
    public class CandidateManagerInitializer
        : CreateDatabaseIfNotExists<CandidateManagerIdentityContext>
    {
        protected override void Seed(CandidateManagerIdentityContext context)
        {
            var passwordHasher = new PasswordHasher();

            var roles = new Dictionary<string, IdentityRole>
            {
                {"Admin", new IdentityRole("Admin")}
            };

            var users = new Dictionary<string, CandidateManagerUser>
            {
                {
                    "DefaultAdmin", new CandidateManagerUser
                    {
                        UserName = ConfigurationManager.AppSettings["defaultAdminUserEmail"],
                        Email = ConfigurationManager.AppSettings["defaultAdminUserEmail"],
                        PasswordHash = passwordHasher.HashPassword(
                            ConfigurationManager.AppSettings["defaultAdminUserPassword"]),
                        SecurityStamp = Guid.NewGuid().ToString()
                    }
                }
            };

            var defaultAdminUser = users["DefaultAdmin"];
            roles.Values.ForEach(role => defaultAdminUser.Roles.Add(new IdentityUserRole
            {
                UserId = defaultAdminUser.Id,
                RoleId = role.Id
            }));

            roles.Values.ForEach(role => context.Roles.Add(role));
            users.Values.ForEach(user => context.Users.Add(user));
        }
    }
}
