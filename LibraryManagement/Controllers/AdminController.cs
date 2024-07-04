using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LibraryManagement.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminLogin(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // Perform admin login logic here
                // For example, check if the user is an admin and the credentials are correct
                bool isAdmin = CheckIfAdmin(model.Email, model.Password);

                if (isAdmin)
                {
                    // If login is successful, redirect to the Admin page
                    return RedirectToAction("Admin", "Admin");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private bool CheckIfAdmin(string email, string password)
        {
            // Replace this with your actual admin validation logic
            // For example, you could check against a database or an in-memory list
            return email == "admin@example.com" && password == "adminpassword";
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("AdminLogin", "Admin");
        }
    }
}