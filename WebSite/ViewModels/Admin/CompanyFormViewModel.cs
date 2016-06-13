using BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

using AutoMapper;


namespace WebSite.ViewModels.Admin
{
    public class CompanyFormViewModel
    {
        public int? Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}