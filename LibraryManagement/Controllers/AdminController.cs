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
                // Perform login logic here

                // If login is successful, redirect to another page
                return RedirectToAction("AdminHome", "Admin");
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

        public ActionResult AdminHome()
        {
            return View();
        }
        public void AddUser(Signup model)
        {
            // Add user to database logic here
           // string query = $"INSERT INTO Users (Name, Email, Password) VALUES ('{model.Name}', '{model.Email}', '{model.Password}')";
            //db.SetData(query);
        }

        public void AddContactMessage(contact_messages message)
        {
            // Logic to add contact message
        }
      

        public ActionResult AdminMessage()
        {
            var messages = db.contact_messages.ToList();
            return View(messages);
        }


        public ActionResult AdminBook()
        {
            //string query = "SELECT * FROM Books";
            //DataTable dt = db.GetData(query);
            //List<LibraryManagement.Models.book> books = new List<LibraryManagement.Models.Book>();

            //foreach (DataRow row in dt.Rows)
            //{
            //    books.Add(new LibraryManagement.Models.book
            //    {
            //        Id = Convert.ToInt32(row["Id"]),
            //        Title = row["Title"].ToString(),
            //        Author = row["Author"].ToString(),
            //        Genre = row["Genre"].ToString(),
            //        Language = row["Language"].ToString(),
            //        IsAvailable = Convert.ToBoolean(row["IsAvailable"]),
            //        ImageUrl = row["ImageUrl"].ToString(),
            //        PdfUrl = row["PdfUrl"].ToString(),
            //        Summary = row["Summary"].ToString()
            //    });
            //}

            return View(db.books.ToList());
        }

        [HttpGet]  // This method handles GET requests
        public ActionResult CreateBook()
        {
            return View();
        }

        [HttpPost]  // This method handles POST requests
        [ValidateAntiForgeryToken]
        public ActionResult CreateBook([Bind(Include = "Id,Title,Author,Genre,Language,IsAvailable,ImageUrl,PdfUrl,Summary")] LibraryManagement.Models.book book)
        {
            if (ModelState.IsValid)
            {
                //string query = $"INSERT INTO Books (Title, Author, Genre, Language, IsAvailable, ImageUrl, PdfUrl, Summary) VALUES ('{book.Title}', '{book.Author_Id}', '{book.Genre}', '{book.Language}', '{book.Available}', '{book.ImageUrl}', '{book.PdfUrl}', '{book.Summary}')";
               // db.SetData(query);
                return RedirectToAction("AdminBook");
            }

            return View(book);
        }


        public ActionResult EditBook(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            string query = $"SELECT * FROM Books WHERE Id = {id}";
           // DataTable dt = db.GetData(query);
            //if (dt.Rows.Count == 0)
            //{
            //    return HttpNotFound();
            //}

            //DataRow row = dt.Rows[0];
            //LibraryManagement.Models.Book book = new LibraryManagement.Models.Book
            //{
            //    Id = Convert.ToInt32(row["Id"]),
            //    Title = row["Title"].ToString(),
            //    Author = row["Author"].ToString(),
            //    Genre = row["Genre"].ToString(),
            //    Language = row["Language"].ToString(),
            //    IsAvailable = Convert.ToBoolean(row["IsAvailable"]),
            //    ImageUrl = row["ImageUrl"].ToString(),
            //    PdfUrl = row["PdfUrl"].ToString(),
            //    Summary = row["Summary"].ToString()
            //};

            return View(db.books.Find(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBook([Bind(Include = "Id,Title,Author,Genre,Language,IsAvailable,ImageUrl,PdfUrl,Summary")] LibraryManagement.Models.book book)
        {
            if (ModelState.IsValid)
            {
                //string query = $"UPDATE Books SET Title = '{book.Title}', Author = '{book.Author}', Genre = '{book.Genre}', Language = '{book.Language}', IsAvailable = '{book.IsAvailable}', ImageUrl = '{book.ImageUrl}', PdfUrl = '{book.PdfUrl}', Summary = '{book.Summary}' WHERE Id = {book.Id}";
                //db.SetData(query);
                return RedirectToAction("AdminBook");
            }

            return View(book);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            //string query = $"SELECT * FROM Books WHERE Id = {id}";
            //DataTable dt = db.GetData(query);
            //if (dt.Rows.Count == 0)
            //{
            //    return HttpNotFound();
            //}

            //DataRow row = dt.Rows[0];
            //LibraryManagement.Models.book book = new LibraryManagement.Models.Book
            //{
            //    Id = Convert.ToInt32(row["Id"]),
            //    Title = row["Title"].ToString(),
            //    Author = row["Author"].ToString(),
            //    Genre = row["Genre"].ToString(),
            //    Language = row["Language"].ToString(),
            //    IsAvailable = Convert.ToBoolean(row["IsAvailable"]),
            //    ImageUrl = row["ImageUrl"].ToString(),
            //    PdfUrl = row["PdfUrl"].ToString(),
            //    Summary = row["Summary"].ToString()
            //};

            return View(db.books.Find(id));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            string query = $"DELETE FROM Books WHERE Id = {id}";
            //db.SetData(query);
            return RedirectToAction("AdminBook");
        }

        //private static List<LibraryManagement.Models.Author> authors = new List<LibraryManagement.Models.Author>
        //{
        //    new LibraryManagement.Models.Author { Id = 1, Name = "Ana Huang", About = "Ana Huang is an Amazon best-selling author of Young Adult and contemporary romance.", ImageUrl = "/Content/Images/author1.jpg", Books = new List<LibraryManagement.Models.Book>
        //    {
        //        new LibraryManagement.Models.Book { Id = 1, Title = "Twisted Love" },
        //    }},
        //    new LibraryManagement.Models.Author { Id = 2, Name = "Colleen Hoover", About = "Colleen Hoover is an American author who primarily writes novels in the romance and young adult fiction genres.", ImageUrl = "/Content/Images/author2.jpg", Books = new List<LibraryManagement.Models.Book>
        //    {
        //        new LibraryManagement.Models.Book { Id = 2, Title = "It Ends with Us" },
        //    }},
        //    new LibraryManagement.Models.Author { Id = 3, Name = "Ali Hazelwood", About = "Ali Hazelwood is a romance author with a love for all things STEM.", ImageUrl = "/Content/Images/author3.jpg", Books = new List<LibraryManagement.Models.Book>
        //    {
        //        new LibraryManagement.Models.Book { Id = 3, Title = "Love on the Brain" },
        //    }},
        //    new LibraryManagement.Models.Author { Id = 4, Name = "Freida McFadden", About = "Freida McFadden is a practicing physician specializing in brain injury who writes medical humor and suspense novels.", ImageUrl = "/Content/Images/author4.jpg", Books = new List<LibraryManagement.Models.Book>
        //    {
        //        new LibraryManagement.Models.Book { Id = 4, Title = "The Housemaid" },
        //    }},
        //    new LibraryManagement.Models.Author { Id = 5, Name = "J. K. Rowling", About = "J.K. Rowling is a British author, best known for writing the Harry Potter fantasy series.", ImageUrl = "/Content/Images/author5.jpg", Books = new List<LibraryManagement.Models.Book>
        //    {
        //        new LibraryManagement.Models.Book { Id = 5, Title = "Harry Potter and the Philosopher's Stone" },
        //    }},
        //    new LibraryManagement.Models.Author { Id = 6, Name = "Agatha Christie", About = "Agatha Christie was an English writer known for her 66 detective novels and 14 short story collections.", ImageUrl = "/Content/Images/author6.jpg", Books = new List<LibraryManagement.Models.Book>
        //    {
        //        new LibraryManagement.Models.Book { Id = 6, Title = "Murder on the Orient Express" },
        //    }},
        //};
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddBorrowRequest(BorrowBook model)
        {
            if (ModelState.IsValid)
            {
                // Logic to handle the borrow request
                // For example, saving the request to a database

                // Redirect or return a view after handling the request
                return RedirectToAction("Index");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult AdminAuthor()
        {
            return View(db.authors.ToList());
        }

        public ActionResult EditAuthor(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            LibraryManagement.Models.author author = db.authors.FirstOrDefault(a => a.Id == id);
            if (author == null)
            {
                return HttpNotFound();
            }

            return View(author);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAuthor([Bind(Include = "Id,Name,About,Image")] LibraryManagement.Models.author author)
        {
            if (ModelState.IsValid)
            {
                var existingAuthor = db.authors.FirstOrDefault(a => a.Id == author.Id);
                if (existingAuthor != null)
                {
                    existingAuthor.Name = author.Name;
                    existingAuthor.About = author.About;
                    existingAuthor.Image = author.Image;
                }

                return RedirectToAction("AdminAuthor");
            }

            return View(author);
        }
        public ActionResult AdminWishlist()
        {
            return View();
        }
        
        public ActionResult DeleteAuthor(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            LibraryManagement.Models.author author = db.authors.FirstOrDefault(a => a.Id == id);
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
                return RedirectToAction("AdminAuthor");
            }

            return View(author);
        }

        public ActionResult AdminUserList()
        {
            var users = db.Signups.ToList();
            return View(users);
        }

    }
}
