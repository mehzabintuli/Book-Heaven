using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using static LibraryManagement.Controllers.AdminController;

namespace LibraryManagement.Controllers
{
    public class AdminController : Controller
    {


        private static List<Book> books = new List<Book>
        {
            new Book { Id = 1, Title = "Twisted Love", Author = "Ana Huang", Genre = "Romance Novel", Language = "English", IsAvailable = true, ImageUrl = "/Content/Images/book1.jpg", PdfUrl = "/Content/Pdfs/book1.pdf", Summary = "A captivating romance story." },
            new Book { Id = 2, Title = "It Ends with Us", Author = "Colleen Hoover", Genre = "Contemporary Romance", Language = "English", IsAvailable = true, ImageUrl = "/Content/Images/book2.jpg", PdfUrl = "/Content/Pdfs/book2.pdf", Summary = "A story of love and resilience." },
            new Book { Id = 3, Title = "Love on the Brain", Author = "Ali Hazelwood", Genre = "Romance Novel", Language = "English", IsAvailable = false, ImageUrl = "/Content/Images/book3.jpg", PdfUrl = "/Content/Pdfs/book3.pdf", Summary = "A journey through the complexities of love." },
            new Book { Id = 4, Title = "The Housemaid", Author = "Freida McFadden", Genre = "Thriller", Language = "English", IsAvailable = true, ImageUrl = "/Content/Images/book4.jpg", PdfUrl = "/Content/Pdfs/book4.pdf", Summary = "A gripping thriller that keeps you on the edge of your seat." },
            new Book { Id = 5, Title = "Harry Potter and the Philosopher's Stone", Author = "J. K. Rowling", Genre = "Fantasy Fiction", Language = "English", IsAvailable = false, ImageUrl = "/Content/Images/book5.jpg", PdfUrl = "/Content/Pdfs/book5.pdf", Summary = "The magical journey of a young wizard." },
            new Book { Id = 6, Title = "Murder on the Orient Express", Author = "Agatha Christie", Genre = "Detective Fiction", Language = "English", IsAvailable = true, ImageUrl = "/Content/Images/book6.jpg", PdfUrl = "/Content/Pdfs/book6.pdf", Summary = "A classic mystery novel with unexpected twists." }
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

        public ActionResult AdminBook()
        {

            return View(books);
        }

        public ActionResult CreateBook()
        {
            //return RedirectToAction("Create", "Admin");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBook([Bind(Include = "Id,Title,Author,Genre,Language,IsAvailable,ImageUrl,PdfUrl")] Book book)
        {
            if (ModelState.IsValid)
            {
                book.Id = books.Count > 0 ? books.Max(b => b.Id) + 1 : 1;
                books.Add(book);
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

            Book book = books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return HttpNotFound();
            }

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBook([Bind(Include = "Id,Title,Author,Genre,Language,IsAvailable,ImageUrl,PdfUrl")] Book book)
        {
            if (ModelState.IsValid)
            {
                var existingBook = books.FirstOrDefault(b => b.Id == book.Id);
                if (existingBook != null)
                {
                    existingBook.Title = book.Title;
                    existingBook.Author = book.Author;
                    existingBook.Genre = book.Genre;
                    existingBook.Language = book.Language;
                    existingBook.IsAvailable = book.IsAvailable;
                    existingBook.ImageUrl = book.ImageUrl;
                    existingBook.PdfUrl = book.PdfUrl;
                }
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

            Book book = books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return HttpNotFound();
            }

            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                books.Remove(book);
            }
            return RedirectToAction("AdminBook");
        }


        private static List<Author> authors = new List<Author>
        {
            new Author { Id = 1, Name = "Ana Huang", About = "Ana Huang is an Amazon best-selling author of Young Adult and contemporary romance.", ImageUrl = "/Content/Images/author1.jpg", Books = new List<Book>
            {
                new Book { Id = 1, Title = "Twisted Love" },
            }},
            new Author { Id = 2, Name = "Colleen Hoover", About = "Colleen Hoover is an American author who primarily writes novels in the romance and young adult fiction genres.", ImageUrl = "/Content/Images/author2.jpg", Books = new List<Book>
            {
                new Book { Id = 2, Title = "It Ends with Us" },
            }},
            new Author { Id = 3, Name = "Ali Hazelwood", About = "Ali Hazelwood is the #1 New York Times bestselling author.", ImageUrl = "/Content/Images/author3.jpg", Books = new List<Book>
            {
                new Book { Id = 3, Title = "Love on the Brain" },
            }},
        };

        public ActionResult AdminAuthor()
        {
            return View(authors);
        }

        // GET: Admin/CreateAuthor
        public ActionResult CreateAuthor()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAuthor([Bind(Include = "Id,Name,About,ImageUrl,Books")] Author author)
        {
            if (ModelState.IsValid)
            {
                author.Id = authors.Count > 0 ? authors.Max(a => a.Id) + 1 : 1;
                authors.Add(author);
                return RedirectToAction("AdminAuthor");
            }

            return View(author);
        }





        // GET: Admin/EditAuthor/5
        public ActionResult EditAuthor(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Author author = authors.FirstOrDefault(a => a.Id == id);
            if (author == null)
            {
                return HttpNotFound();
            }

            return View(author);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAuthor([Bind(Include = "Id,Name,About,ImageUrl,Books")] Author author)
        {
            if (ModelState.IsValid)
            {
                var existingAuthor = authors.FirstOrDefault(a => a.Id == author.Id);
                if (existingAuthor != null)
                {
                    existingAuthor.Name = author.Name;
                    existingAuthor.About = author.About;
                    existingAuthor.ImageUrl = author.ImageUrl;
                    existingAuthor.Books = author.Books;
                }
                return RedirectToAction("AdminAuthor");
            }

            return View(author);
        }

        private Author GetAuthorById(int id)
        {
            // Your logic to fetch the author from the database
            return new Author
            {
                Id = id,
                Name = "Author Name",
                About = "About the Author",
                ImageUrl = "url_to_image",
                Books = new List<Book> {
                    new Book { Title = "Book 1" },
                    new Book { Title = "Book 2" }
                }
            };
        }






        // GET: Admin/DeleteAuthor/5
        public ActionResult DeleteAuthor(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Author author = authors.FirstOrDefault(a => a.Id == id);
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
            Author author = authors.FirstOrDefault(a => a.Id == id);
            if (author != null)
            {
                authors.Remove(author);
            }
            return RedirectToAction("AdminAuthor");
        }

        // Other existing actions...

        public class Author
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string About { get; set; }
            public string ImageUrl { get; set; }
            public List<Book> Books { get; set; }
        }

        private static List<ContactMessage> contactMessages = new List<ContactMessage>();

        public ActionResult AdminMessage()
        {
            return View(contactMessages);
        }

        // Method to handle new contact messages from the HomeController
        public void AddContactMessage(ContactMessage message)
        {
            message.Id = contactMessages.Count > 0 ? contactMessages.Max(m => m.Id) + 1 : 1;
            contactMessages.Add(message);
        }


        private static List<HomeController.BorrowModel> borrowRequests = new List<HomeController.BorrowModel>();

        public ActionResult AdminBorrowlist()
        {
            return View(borrowRequests);
        }

        public void AddBorrowRequest(HomeController.BorrowModel borrowModel)
        {
            borrowRequests.Add(borrowModel);
        }


        private static List<SignUpModel> users = new List<SignUpModel>();


        public void AddUser(SignUpModel user)
        {
            users.Add(user);
        }
        public ActionResult AdminUserlist()
        {
            return View(users);
        }





    }
}
