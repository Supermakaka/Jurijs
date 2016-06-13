using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

using BusinessLogic.Models;

namespace WebSite.ViewModels.Admin
{
    public class EditUserViewModel
    {
        public int Id { get; set; }

        public IEnumerable<CompanyListDatatableViewModel> Companies { get; set; }

        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        [Display(Name = "New Password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        public virtual string Email { get; set; }

        public bool Disabled { get; set; }

        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }
    }
}