using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Security.Claims;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BusinessLogic.Models
{
    public class User : IdentityUser<int, UserLogin, UserRole, UserClaim>
    {
        public bool Disabled { get; set; }

        public virtual int? CompanyId { get; set; }

        public virtual Company Company { get; set; }

        public string FullName { get; set; }

        public DateTime CreateDate { get; set; }

        public User() : base()
        {
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            // Add custom user claims here

            return userIdentity;
        }
    }
}
