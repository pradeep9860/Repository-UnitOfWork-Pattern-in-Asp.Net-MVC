using HRTest_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRTest_MVC.ViewModel
{
    public class UserWithRoleViewModel
    {
        public User  User { get; set; }
        public Roles Roles { get; set; }
    }
}