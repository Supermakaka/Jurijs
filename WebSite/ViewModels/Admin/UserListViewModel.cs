using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

using BusinessLogic.Models;

namespace WebSite.ViewModels.Admin
{
    using Mapping;

    public class UserListViewModel
    {
        public int? Role { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }
    }
}