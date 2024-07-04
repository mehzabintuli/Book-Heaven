using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryManagement.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // Perform login logic here

                // If login is successful, redirect to another page
                return RedirectToAction("Index", "Home");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: Account/SignUp
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(SignUpModel model)
        {
            if (ModelState.IsValid)
            {
                // Perform sign-up logic here
                // For example, save the user details to a database

                // If sign-up is successful, redirect to login page
                return RedirectToAction("Login");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: Account/Logout
        public ActionResult Logout()
        {
            // Clear the user's session
            Session.Clear();
            Session.Abandon();

            // Redirect to login page
            return RedirectToAction("Login");
        }

    }
}