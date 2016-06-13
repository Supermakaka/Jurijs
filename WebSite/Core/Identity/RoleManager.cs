using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.AspNet.Identity;

using BusinessLogic.Models;

namespace WebSite.Core.Identity
{
    public class IdentityRoleManager : RoleManager<Role, int>
    {
        public IdentityRoleManager(IRoleStore<Role, int> roleStore)
            : base(roleStore)
        {
        }
    }
}