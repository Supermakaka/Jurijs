using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;

using BusinessLogic.Models;

namespace BusinessLogic.Services
{
    public class CompanyService : BaseService<Company>, ICompanyService
    {
        public CompanyService(IDataContext dataContext)
            : base(dataContext)
        {
        }
    }

    public interface ICompanyService : IService<Company>
    {
    }
}
