﻿using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.IO;

namespace LibraryManagement.Controllers
{
    public class AdminController : Controller
    {
        private Database db = new Database();

        private static List<LibraryManagement.Models.Book> books = new List<LibraryManagement.Models.Book>
        {
            new LibraryManagement.Models.Book { Id = 1, Title = "Twisted Love", Author = "Ana Huang", Genre = "Romance Novel", Language = "English", IsAvailable = true, ImageUrl = "/Content/Images/book1.jpg", PdfUrl = "/Content/Pdfs/book1.pdf", Summary = "A captivating romance story." },
            new LibraryManagement.Models.Book { Id = 2, Title = "It Ends with Us", Author = "Colleen Hoover", Genre = "Contemporary Romance", Language = "English", IsAvailable = true, ImageUrl = "/Content/Images/book2.jpg", PdfUrl = "/Content/Pdfs/book2.pdf", Summary = "A story of love and resilience." },
            new LibraryManagement.Models.Book { Id = 3, Title = "Love on the Brain", Author = "Ali Hazelwood", Genre = "Romance Novel", Language = "English", IsAvailable = false, ImageUrl = "/Content/Images/book3.jpg", PdfUrl = "/Content/Pdfs/book3.pdf", Summary = "A journey through the complexities of love." },
            new LibraryManagement.Models.Book { Id = 4, Title = "The Housemaid", Author = "Freida McFadden", Genre = "Thriller", Language = "English", IsAvailable = true, ImageUrl = "/Content/Images/book4.jpg", PdfUrl = "/Content/Pdfs/book4.pdf", Summary = "A gripping thriller that keeps you on the edge of your seat." },
            new LibraryManagement.Models.Book { Id = 5, Title = "Harry Potter and the Philosopher's Stone", Author = "J. K. Rowling", Genre = "Fantasy Fiction", Language = "English", IsAvailable = false, ImageUrl = "/Content/Images/book5.jpg", PdfUrl = "/Content/Pdfs/book5.pdf", Summary = "The magical journey of a young wizard." },
            new LibraryManagement.Models.Book { Id = 6, Title = "Murder on the Orient Express", Author = "Agatha Christie", Genre = "Detective Fiction", Language = "English", IsAvailable = true, ImageUrl = "/Content/Images/book6.jpg", PdfUrl = "/Content/Pdfs/book6.pdf", Summary = "A classic mystery novel with unexpected twists." }
        };

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
        public void AddUser(SignUpModel model)
        {
            // Add user to database logic here
            string query = $"INSERT INTO Users (Name, Email, Password) VALUES ('{model.Name}', '{model.Email}', '{model.Password}')";
            db.SetData(query);
        }

        public void AddContactMessage(ContactMessage message)
        {
            // Logic to add contact message
        }

        
        public ActionResult AdminBook()
        {
            string query = "SELECT * FROM Books";
            DataTable dt = db.GetData(query);
            List<LibraryManagement.Models.Book> books = new List<LibraryManagement.Models.Book>();

            foreach (DataRow row in dt.Rows)
            {
                books.Add(new LibraryManagement.Models.Book
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Title = row["Title"].ToString(),
                    Author = row["Author"].ToString(),
                    Genre = row["Genre"].ToString(),
                    Language = row["Language"].ToString(),
                    IsAvailable = Convert.ToBoolean(row["IsAvailable"]),
                    ImageUrl = row["ImageUrl"].ToString(),
                    PdfUrl = row["PdfUrl"].ToString(),
                    Summary = row["Summary"].ToString()
                });
            }

            return View(books);
        }

        [HttpGet]  // This method handles GET requests
        public ActionResult CreateBook()
        {
            return View();
        }

        [HttpPost]  // This method handles POST requests
        [ValidateAntiForgeryToken]
        public ActionResult CreateBook([Bind(Include = "Id,Title,Author,Genre,Language,IsAvailable,ImageUrl,PdfUrl,Summary")] LibraryManagement.Models.Book book)
        {
            if (ModelState.IsValid)
            {
                string query = $"INSERT INTO Books (Title, Author, Genre, Language, IsAvailable, ImageUrl, PdfUrl, Summary) VALUES ('{book.Title}', '{book.Author}', '{book.Genre}', '{book.Language}', '{book.IsAvailable}', '{book.ImageUrl}', '{book.PdfUrl}', '{book.Summary}')";
                db.SetData(query);
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
            DataTable dt = db.GetData(query);
            if (dt.Rows.Count == 0)
            {
                return HttpNotFound();
            }

            DataRow row = dt.Rows[0];
            LibraryManagement.Models.Book book = new LibraryManagement.Models.Book
            {
                Id = Convert.ToInt32(row["Id"]),
                Title = row["Title"].ToString(),
                Author = row["Author"].ToString(),
                Genre = row["Genre"].ToString(),
                Language = row["Language"].ToString(),
                IsAvailable = Convert.ToBoolean(row["IsAvailable"]),
                ImageUrl = row["ImageUrl"].ToString(),
                PdfUrl = row["PdfUrl"].ToString(),
                Summary = row["Summary"].ToString()
            };

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBook([Bind(Include = "Id,Title,Author,Genre,Language,IsAvailable,ImageUrl,PdfUrl,Summary")] LibraryManagement.Models.Book book)
        {
            if (ModelState.IsValid)
            {
                string query = $"UPDATE Books SET Title = '{book.Title}', Author = '{book.Author}', Genre = '{book.Genre}', Language = '{book.Language}', IsAvailable = '{book.IsAvailable}', ImageUrl = '{book.ImageUrl}', PdfUrl = '{book.PdfUrl}', Summary = '{book.Summary}' WHERE Id = {book.Id}";
                db.SetData(query);
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

            string query = $"SELECT * FROM Books WHERE Id = {id}";
            DataTable dt = db.GetData(query);
            if (dt.Rows.Count == 0)
            {
                return HttpNotFound();
            }

            DataRow row = dt.Rows[0];
            LibraryManagement.Models.Book book = new LibraryManagement.Models.Book
            {
                Id = Convert.ToInt32(row["Id"]),
                Title = row["Title"].ToString(),
                Author = row["Author"].ToString(),
                Genre = row["Genre"].ToString(),
                Language = row["Language"].ToString(),
                IsAvailable = Convert.ToBoolean(row["IsAvailable"]),
                ImageUrl = row["ImageUrl"].ToString(),
                PdfUrl = row["PdfUrl"].ToString(),
                Summary = row["Summary"].ToString()
            };

            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            string query = $"DELETE FROM Books WHERE Id = {id}";
            db.SetData(query);
            return RedirectToAction("AdminBook");
        }

        private static List<LibraryManagement.Models.Author> authors = new List<LibraryManagement.Models.Author>
        {
            new LibraryManagement.Models.Author { Id = 1, Name = "Ana Huang", About = "Ana Huang is an Amazon best-selling author of Young Adult and contemporary romance.", ImageUrl = "/Content/Images/author1.jpg", Books = new List<LibraryManagement.Models.Book>
            {
                new LibraryManagement.Models.Book { Id = 1, Title = "Twisted Love" },
            }},
            new LibraryManagement.Models.Author { Id = 2, Name = "Colleen Hoover", About = "Colleen Hoover is an American author who primarily writes novels in the romance and young adult fiction genres.", ImageUrl = "/Content/Images/author2.jpg", Books = new List<LibraryManagement.Models.Book>
            {
                new LibraryManagement.Models.Book { Id = 2, Title = "It Ends with Us" },
            }},
            new LibraryManagement.Models.Author { Id = 3, Name = "Ali Hazelwood", About = "Ali Hazelwood is a romance author with a love for all things STEM.", ImageUrl = "/Content/Images/author3.jpg", Books = new List<LibraryManagement.Models.Book>
            {
                new LibraryManagement.Models.Book { Id = 3, Title = "Love on the Brain" },
            }},
            new LibraryManagement.Models.Author { Id = 4, Name = "Freida McFadden", About = "Freida McFadden is a practicing physician specializing in brain injury who writes medical humor and suspense novels.", ImageUrl = "/Content/Images/author4.jpg", Books = new List<LibraryManagement.Models.Book>
            {
                new LibraryManagement.Models.Book { Id = 4, Title = "The Housemaid" },
            }},
            new LibraryManagement.Models.Author { Id = 5, Name = "J. K. Rowling", About = "J.K. Rowling is a British author, best known for writing the Harry Potter fantasy series.", ImageUrl = "/Content/Images/author5.jpg", Books = new List<LibraryManagement.Models.Book>
            {
                new LibraryManagement.Models.Book { Id = 5, Title = "Harry Potter and the Philosopher's Stone" },
            }},
            new LibraryManagement.Models.Author { Id = 6, Name = "Agatha Christie", About = "Agatha Christie was an English writer known for her 66 detective novels and 14 short story collections.", ImageUrl = "/Content/Images/author6.jpg", Books = new List<LibraryManagement.Models.Book>
            {
                new LibraryManagement.Models.Book { Id = 6, Title = "Murder on the Orient Express" },
            }},
        };
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddBorrowRequest(BorrowRequestModel model)
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
            return View(authors);
        }

        public ActionResult EditAuthor(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            LibraryManagement.Models.Author author = authors.FirstOrDefault(a => a.Id == id);
            if (author == null)
            {
                return HttpNotFound();
            }

            return View(author);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAuthor([Bind(Include = "Id,Name,About,ImageUrl")] LibraryManagement.Models.Author author)
        {
            if (ModelState.IsValid)
            {
                var existingAuthor = authors.FirstOrDefault(a => a.Id == author.Id);
                if (existingAuthor != null)
                {
                    existingAuthor.Name = author.Name;
                    existingAuthor.About = author.About;
                    existingAuthor.ImageUrl = author.ImageUrl;
                }

                return RedirectToAction("AdminAuthor");
            }

            return View(author);
        }

        public ActionResult DeleteAuthor(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            LibraryManagement.Models.Author author = authors.FirstOrDefault(a => a.Id == id);
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
            var author = authors.FirstOrDefault(a => a.Id == id);
            if (author != null)
            {
                authors.Remove(author);
            }

            return RedirectToAction("AdminAuthor");
        }

        public ActionResult CreateAuthor()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAuthor([Bind(Include = "Id,Name,About,ImageUrl")] LibraryManagement.Models.Author author)
        {
            if (ModelState.IsValid)
            {
                authors.Add(author);
                return RedirectToAction("AdminAuthor");
            }

            return View(author);
        }
    }
}
