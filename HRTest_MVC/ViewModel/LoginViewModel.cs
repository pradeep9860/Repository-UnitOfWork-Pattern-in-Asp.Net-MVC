using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRTest_MVC.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Email is Requird.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Requird.")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}