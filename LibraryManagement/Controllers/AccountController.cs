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
                // Check if the login is from the admin (no hashing required for admin)
                if (model.Email == "admin@gmail.com" && model.Password == "9900")
                {
                    Session["UserEmail"] = model.Email;
                    return RedirectToAction("AdminBook", "Admin");
                }

                // Hash the entered password for regular users
                var hashedPassword = HashPassword(model.Password);

                // Check if the login is from a regular user (hashed password comparison)
                var user = db.Signups.FirstOrDefault(u => u.Email == model.Email && u.Password == hashedPassword);

                if (user != null)
                {
                    // Set session and redirect to Home page
                    Session["UserEmail"] = user.Email;
                    return RedirectToAction("Index", "Home");
                }

                // If no user is found, show error
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
                    // Hash the password before saving it
                    model.Password = HashPassword(model.Password);

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