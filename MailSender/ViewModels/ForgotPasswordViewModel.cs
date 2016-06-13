using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessageServices.ViewModels
{
    public class ForgotPasswordViewModel
    {
        public int UserId { get; set; }

        public string Code { get; set; }
    }
}