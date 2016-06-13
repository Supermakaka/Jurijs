using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;

using BusinessLogic.Models;

namespace BusinessLogic.Services
{
    public class UserService : BaseService<User>, IUserService
    {
        public UserService(IDataContext dataContext)
            : base(dataContext)
        {
        }

        public void DisableOrEnable(User user)
        {
            user.Disabled = !user.Disabled;

            dataContext.SaveChanges();
        }

        public IQueryable<User> GetAllWithCompanies()
        {
            return dataContext.Users.Include(u => u.Company);
        }
    }

    public interface IUserService : IService<User>
    {
        void DisableOrEnable(User user);
        IQueryable<User> GetAllWithCompanies();
    }
}
