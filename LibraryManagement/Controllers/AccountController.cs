using LibraryManagement.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;

namespace LibraryManagement.Controllers
{
    public class AccountController : Controller
    {
        public class BookHeaven : DbContext
        {
            public BookHeaven() : base("name=BookHeavenEntities")
            {
            }

            public DbSet<Signup> Signups { get; set; }
            public DbSet<Login> Logins { get; set; }
        }

        private BookHeavenEntities db = new BookHeavenEntities();

        // GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login model)
        {
            if (ModelState.IsValid)
            {
                var hashedPassword = HashPassword(model.PasswordHash);
                var user = db.Signups.FirstOrDefault(u => u.Email == model.Email && u.Password == hashedPassword);
                if (user != null)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid login attempt.");
            }

            return View(model);
        }

        // GET: Account/SignUp
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(Signup model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //model.Password = HashPassword(model.Password);
                    db.Signups.Add(model);
                    db.SaveChanges();

                    ViewBag.SuccessMessage = "Sign up successful! You will be redirected to the login page.";
                    return View();
                }
                catch (DbUpdateException ex)
                {
                    var innerException = ex.InnerException?.InnerException;
                    ModelState.AddModelError("", "An error occurred while updating the database: " + innerException?.Message ?? ex.Message);
                }
            }

            return View(model);
        }

        // GET: Account/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Landing", "Account");
        }

        public ActionResult Landing()
        {
            ViewBag.Title = "Welcome";
            return View();
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}
