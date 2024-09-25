using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.IO;
using System.Data.Entity;
using static LibraryManagement.Controllers.HomeController;

namespace LibraryManagement.Controllers
{
    public class AdminController : Controller
    {
        public class BookHeaven : DbContext
        {
            public BookHeaven() : base("name=BookHeavenEntities")
            {
            }

            public DbSet<contact_messages> contact_messages { get; set; }
        }

        BookHeavenEntities db = new BookHeavenEntities();

        public ActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminLogin(Login model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("AdminHome", "Admin");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private bool CheckIfAdmin(string email, string password)
        {
            return email == "admin@example.com" && password == "adminpassword";
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("AdminLogin", "Admin");
        }

        public ActionResult AdminHome()
        {

            return View();
        }

        public void AddUser(Signup model)
        {
            // Code for adding a user
        }

        public void AddContactMessage(contact_messages message)
        {
            // Code for adding a contact message
        }

        public ActionResult AdminMessage()
        {
            var messages = db.contact_messages.ToList();
            return View(messages);
        }


        public ActionResult AdminBook(int page = 1, int pageSize = 8)
        {
            // Retrieve all books
            var books = db.books.ToList();

            // Calculate total number of books
            int totalBooks = books.Count;

            // Fetch the current page of books
            var pagedBooks = books.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Calculate the total number of pages
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalBooks / pageSize);
            ViewBag.CurrentPage = page;

            return View(pagedBooks);  // Return only the paged books to the view
        }


        [HttpGet]
        public ActionResult CreateBook()
        {

            var authors = db.authors.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name
            }).ToList();

            ViewBag.AuthorList = authors;  // Pass the list to the view using ViewBag

            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBook([Bind(Include = "Id,Title,Author_Id,Genre,Language,Available,Cover_Image,PDF_Link,Summary")] LibraryManagement.Models.book book)
        {
            if (ModelState.IsValid)
            {
                db.books.Add(book);
                db.SaveChanges();
                return RedirectToAction("AdminBook");
            }
            else
            {
                // Check for errors
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
                }
            }

            return View(book);
        }

        public ActionResult EditBook(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var book = db.books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }

            // Pass the list of authors to the view to populate the dropdown
            ViewBag.AuthorList = db.authors.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name
            }).ToList();

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBook([Bind(Include = "Id,Title,Author_Id,Genre,Language,Available,Cover_Image,PDF_Link,Summary")] book book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AdminBook");
            }

            // If the model state is invalid, repopulate the author list and return the view
            ViewBag.AuthorList = db.authors.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name
            }).ToList();

            return View(book);
        }


        public ActionResult DeleteBook(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var book = db.books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }

            return View(book);
        }
        [HttpPost, ActionName("DeleteBook")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var book = db.books.Find(id);
            if (book != null)
            {
                db.books.Remove(book);
                db.SaveChanges();
            }
            return RedirectToAction("AdminBook");
        }



        public ActionResult AdminAuthor()
        {
            var authors = db.authors.ToList();
            return View(authors);
        }

        public ActionResult EditAuthor(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var author = db.authors.FirstOrDefault(a => a.Id == id);
            if (author == null)
            {
                return HttpNotFound();
            }

            return View(author);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAuthor([Bind(Include = "Id,Name,About,Image")] author author)
        {
            if (ModelState.IsValid)
            {

                db.Entry(author).State = EntityState.Modified;
                db.SaveChanges();


                return RedirectToAction("AdminAuthor");
            }

            return View(author);
        }


        public ActionResult AdminWishlist()
        {
            // Fetch data from the Wishlist table
            var wishlists = db.Wishlists.ToList();
            return View(wishlists);
        }

        public ActionResult DeleteAuthor(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var author = db.authors.FirstOrDefault(a => a.Id == id);
            if (author == null)
            {
                return HttpNotFound();
            }

            return View(author);
        }

        [HttpPost, ActionName("DeleteAuthor")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAuthorConfirmed(int id)
        {
            var author = db.authors.FirstOrDefault(a => a.Id == id);
            if (author != null)
            {
                db.authors.Remove(author);
                db.SaveChanges();
            }

            return RedirectToAction("AdminAuthor");
        }
        public ActionResult CreateAuthor()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAuthor([Bind(Include = "Id,Name,About,Image")] LibraryManagement.Models.author author)
        {
            if (ModelState.IsValid)
            {
                db.authors.Add(author);
                db.SaveChanges();
                return RedirectToAction("AdminAuthor");
            }

            return View(author);
        }


        public ActionResult AdminUserList()
        {
            var users = db.Signups.ToList();
            return View(users);
        }

        public ActionResult AdminBorrowList()
        {
            var borrowRequests = db.BorrowBooks.ToList();
            return View(borrowRequests);
        }

        [HttpPost]
        public ActionResult SendReply(string email, string message)
        {


            System.Diagnostics.Debug.WriteLine("Reply sent to: " + email);
            System.Diagnostics.Debug.WriteLine("Message: " + message);

            TempData["SuccessMessage"] = "Reply sent successfully!";
            return RedirectToAction("AdminMessage");
        }


        public ActionResult AdminReturnBook()
        {
            var returnBooks = db.ReturnBooks.ToList();
            return View(returnBooks);
        }




    }
}