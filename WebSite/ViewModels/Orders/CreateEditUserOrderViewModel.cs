using BusinessLogic.Models;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebSite.ViewModels.Orders
{
    using ViewModels.Admin;

    public class CreateEditUserOrderViewModel
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        
        [DisplayName("User")]
        public int UserId { get; set; }
        public IEnumerable<UserListViewModel> Users { get; set; }
    }
}