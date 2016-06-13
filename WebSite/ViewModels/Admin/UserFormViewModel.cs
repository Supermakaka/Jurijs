using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using ExpressiveAnnotations.Attributes;

using BusinessLogic.Models;

namespace WebSite.ViewModels.Admin
{
    public class UserFormViewModel
    {
        public int? Id { get; set; }

        public IEnumerable<SelectListItem> Companies { get; set; }

        [Display(Name = "Company")]
        public int? CompanyId { get; set; }

        [Display(Name = "Full Name")]
        [Required]
        public string FullName { get; set; }

        [RequiredIf("Id == null", ErrorMessage = "The Password field is required.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public virtual string Email { get; set; }

        public bool Disabled { get; set; }
    }
}