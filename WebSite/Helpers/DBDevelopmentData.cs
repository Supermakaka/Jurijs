using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Linq;
using System.Web;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using BusinessLogic.IdentityStores;
using BusinessLogic.Models;
using MessageServices.Identity;

namespace WebSite.Helpers
{
    using Core.Identity;

    public class DBDevelopmentData : DropCreateDatabaseIfModelChanges<DataContext>
    {
        private const string DefaultAdminUserEmail = "admin@example.com";
        private const string DefaultAdminUserPassword = "qwerty";

        private IIdentityEmailService emailService;
        private IIdentitySmsService smsService;

        public DBDevelopmentData(IIdentityEmailService emailService, IIdentitySmsService smsService)
        {
            this.emailService = emailService;
            this.smsService = smsService;
        }

        protected override void Seed(DataContext context)
        {
            CreateCompanies(context);
            CreateRoles(context);
            CreateAdminUser(context);
        }

        /// <summary>
        /// Creates default roles.
        /// </summary>
        public void CreateRoles(DataContext context)
        {
            var roleManager = new IdentityRoleManager(new RoleStore(context));

            if (!roleManager.RoleExists(RoleNames.Admin))
            {
                var result = roleManager.Create(new Role(RoleNames.Admin));

                if (!result.Succeeded)
                    throw new Exception("Error creating admin role: " + result.Errors.FirstOrDefault());
            }

            if (!roleManager.RoleExists(RoleNames.User))
            {
                var result = roleManager.Create(new Role(RoleNames.User));

                if (!result.Succeeded)
                    throw new Exception("Error creating user role: " + result.Errors.FirstOrDefault());
            }
        }

        /// <summary>
        /// Creates default admin user.
        /// </summary>
        public User CreateAdminUser(DataContext context)
        {
            var userManager = new IdentityUserManager(new UserStore(context), emailService, smsService);

            var user = userManager.FindByName(DefaultAdminUserEmail);

            if (user != null)
                return null;

            user = new User { UserName = DefaultAdminUserEmail, Email = DefaultAdminUserEmail, FullName = "Admin", CreateDate = DateTime.Now };

            var result = userManager.Create(user, DefaultAdminUserPassword);

            if (!result.Succeeded)
                throw new Exception("Error creating default admin user: " + result.Errors.FirstOrDefault());

            result = userManager.AddToRoles(user.Id, RoleNames.Admin, RoleNames.User);

            if (!result.Succeeded)
                throw new Exception("Error adding default admin user to the role: " + result.Errors.FirstOrDefault());

            //userManager.AddClaim(user, new Claim("ManageStore", "Allowed"));

            return user;
        }

        private void CreateCompanies(DataContext context)
        {
            List<Company> companies = new List<Company>
            {
                new Company {Id = 1, Name = "Company 1"},
                new Company {Id = 2, Name = "Company 2"}
            };

            // add data into context and save to db
            foreach (Company c in companies)
            {
                context.Companies.Add(c);
            }
        }
    }
}