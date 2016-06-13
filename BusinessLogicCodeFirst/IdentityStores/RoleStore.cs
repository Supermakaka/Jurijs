using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

using Microsoft.AspNet.Identity.EntityFramework;

namespace BusinessLogic.IdentityStores
{
    using Models;

    public class RoleStore : RoleStore<Role, int, UserRole>
    {
        public RoleStore(DataContext context) : base(context) { }
    }
}
