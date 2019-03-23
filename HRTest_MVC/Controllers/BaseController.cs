using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTest_MVC.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        public int GetCurrentUser()
        {
            int x = 0; 
            if (Int32.TryParse(User.Identity.GetUserId(), out x))
            {
                return x;
            }
            return x;
        }
    }
}