using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebSite.ViewModels.Admin
{
    public class UserListDatatableViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Company")]
        public string CompanyName { get; set; }
        public string FullName { get; set; }
        public virtual string Email { get; set; }
        public string Role { get; set; }
        public bool Disabled { get; set; }
        [Display(Name = "Created")]
        public string CreateDate { get; set; }
    }
}