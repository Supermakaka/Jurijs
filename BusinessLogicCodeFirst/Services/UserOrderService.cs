using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;

using BusinessLogic.Models;

namespace BusinessLogic.Services
{
    public class UserOrderService : BaseService<UserOrder>, IUserOrderService
    {
        public UserOrderService(IDataContext dataContext)
            : base(dataContext)
        {
        }

        public IEnumerable<UserOrder> GetAllOrdersWithUsers()
        {
            return dataContext.UserOrders.OfType<UserOrder>()
                .Include(u => u.User)
                .ToList();
        }
    }

    public interface IUserOrderService : IService<UserOrder>
    {
        IEnumerable<UserOrder> GetAllOrdersWithUsers();
    }
}
