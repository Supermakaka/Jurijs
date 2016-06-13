using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AutoMapper;

using BusinessLogic.Models;

namespace WebSite.ViewModels.Mapping
{
    using Core.Identity;

    public class DateTimeToFormattedStringResolver : ValueResolver<DateTime?, string>
    {
        protected override string ResolveCore(DateTime? source)
        {
            if (!source.HasValue)
                return "";

            return source.Value.ToString("MM/dd/yyyy h:mm tt");
        }
    }

    public class DateToFormattedStringResolver : ValueResolver<DateTime?, string>
    {
        protected override string ResolveCore(DateTime? source)
        {
            if (!source.HasValue)
                return "";

            return source.Value.ToString("MM/dd/yyyy");
        }
    }

    public class UserRoleListToStringResolver : ValueResolver<User, string>
    {
        private IdentityUserManager userManager;

        public UserRoleListToStringResolver(IdentityUserManager userManager)
        {
            this.userManager = userManager;
        }

        protected override string ResolveCore(User source)
        {
            var list = userManager.GetRolesAsync(source.Id).Result;

            return String.Join(", ", list);
        }
    }
}