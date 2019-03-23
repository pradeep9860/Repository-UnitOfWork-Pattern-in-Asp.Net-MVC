using HRTest_MVC.ViewModel;
  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc; 
using System.IO;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Web.Security;
using HRTest_MVC.Context;
using ApplicationCore.Helper;
using HRTest_MVC.Persistence;

namespace HRTest_MVC.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            if (string.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null)
                returnUrl = Server.UrlEncode(Request.UrlReferrer.PathAndQuery);

            if (Url.IsLocalUrl(returnUrl) && !string.IsNullOrEmpty(returnUrl))
            {
                ViewBag.ReturnUrl = returnUrl;
            }
            else
            {
                ViewBag.ReturnUrl = "/";
            } 
            return View();
        }

        [HttpPost]
        [ActionName("Login")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Index", "Home");
                }


                if (ModelState.IsValid)
                {
                    using (var unitOfWorks = new UnitOfWork(new PlutoContext()))
                    {
                        var data = unitOfWorks.Users.Login(model);
                        if (data.User != null)
                        {
                            var ident = new ClaimsIdentity(
                                          new[] { 
                                          // adding following 2 claim just for supporting default antiforgery provider
                                          new Claim(ClaimTypes.NameIdentifier, data.User.Id.ToString()),
                                          new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),
                                          new Claim(ClaimTypes.GivenName,model.Email),


                                          new Claim(ClaimTypes.Name, data.User.Email), 
                                          // optionally you could add roles if any
                                          new Claim(ClaimTypes.Role, data.Roles.Name)

                                          },
                                          DefaultAuthenticationTypes.ApplicationCookie);

                            HttpContext.GetOwinContext().Authentication.SignIn(new AuthenticationProperties { IsPersistent = false }, ident);
                            // auth succeed 
                            if (!string.IsNullOrEmpty(returnUrl))
                                return Redirect(returnUrl);
                            return RedirectToAction("Index", "Home");

                        } 
                    }
                }
                @ViewBag.Message = "Email or Password is Invalid";
                return View("Login", model);
            }
            catch (Exception e)
            {
                @ViewBag.Message = "Email or Password is Invalid";
                return View("Login", model);
            }
        }

        public ActionResult Logout()
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut(); 
            return RedirectToAction("Login");
        }
    }
}