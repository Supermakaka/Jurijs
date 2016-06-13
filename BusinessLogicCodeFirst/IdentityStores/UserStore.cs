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

    public class UserStore : UserStore<User, Role, int, UserLogin, UserRole, UserClaim>
    {
        public UserStore(DataContext context) : base(context) { }
    }
}
