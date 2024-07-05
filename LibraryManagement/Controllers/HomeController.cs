using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryManagement.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home/Landing
        public ActionResult Landing()
        {
            return View();
        }

        // Placeholder actions for Admin and User buttons
        public ActionResult Admin()
        {
            // Redirect to Admin page or view
            return RedirectToAction("AdminDashboard", "Admin");
        }

        public ActionResult User()
        {
            // Redirect to User page or view
            return RedirectToAction("UserDashboard", "User");
        }
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact(string Name, string Email, string PhoneNumber, string Subject, string Message)
        {
            // Handle form submission, e.g., send email or save to database
            ViewBag.Message = "Thank you for contacting us! We will get back to you soon.";
            return View();
        }

        public class HomeController
        {
            public ActionResult Contact(string Name, string Email, string PhoneNumber, string Subject, string Message)
            {
                var contactMessage = new ContactMessage
                {

                    Email = Email,
                    Subject = Subject,
                    Message = Message
                };

                // Assuming an instance of AdminController is created to add the message
                AdminController adminController = new AdminController();
                adminController.AddContactMessage(contactMessage);

                // Handle form submission, e.g., send email or save to database
                ViewBag.Message = "Thank you for contacting us! We will get back to you soon.";
                return View();
            }

            public class Book
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Author { get; set; }
            public string Genre { get; set; }
            public string Language { get; set; }
            public bool IsAvailable { get; set; }
            public string ImageUrl { get; set; }
            public string PdfUrl { get; set; }
        }

        public class BorrowModel
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Contact { get; set; }
            public DateTime IssueDate { get; set; }
            public DateTime ReturnDate { get; set; }
            public int BookId { get; set; }
        }

        private List<Book> GetBooks()
        {
            return new List<Book>
            {
                new Book { Id = 1, Title = "Twisted Love", Author = "Ana Huang", Genre = "Romance Novel", Language = "English", IsAvailable = true, ImageUrl = Url.Content("~/Content/Images/book1.jpg"), PdfUrl = Url.Content("~/Content/Pdfs/book1.pdf") },
                new Book { Id = 2, Title = "It Ends with Us", Author = "Colleen Hoover", Genre = "Contemporary Romance", Language = "English", IsAvailable = true, ImageUrl = Url.Content("~/Content/Images/book2.jpg"), PdfUrl = Url.Content("~/Content/Pdfs/book2.pdf") },
                new Book { Id = 3, Title = "Love on the Brain", Author = "Ali Hazelwood", Genre = "Romance Novel", Language = "English", IsAvailable = false, ImageUrl = Url.Content("~/Content/Images/book3.jpg"), PdfUrl = Url.Content("~/Content/Pdfs/book3.pdf") },
                new Book { Id = 4, Title = "The Housemaid", Author = "Freida McFadden", Genre = "Thriller", Language = "English", IsAvailable = true, ImageUrl = Url.Content("~/Content/Images/book4.jpg"), PdfUrl = Url.Content("~/Content/Pdfs/book4.pdf") },
                new Book { Id = 5, Title = "Harry Potter and the Philosopher's Stone", Author = "J. K. Rowling", Genre = "Fantasy Fiction", Language = "English", IsAvailable = false, ImageUrl = Url.Content("~/Content/Images/book5.jpg"), PdfUrl = Url.Content("~/Content/Pdfs/book5.pdf") },
                new Book { Id = 6, Title = "Murder on the Orient Express", Author = "Agatha Christie", Genre = "Detective Fiction", Language = "English", IsAvailable = true, ImageUrl = Url.Content("~/Content/Images/book6.jpg"), PdfUrl = Url.Content("~/Content/Pdfs/book6.pdf") }
            };
        }

        public ActionResult Books(int page = 1)
        {
            int pageSize = 3;
            var books = GetBooks();
            var paginatedBooks = books.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)books.Count / pageSize);

            return View(paginatedBooks);
        }

        [HttpGet]
        public ActionResult Borrow(int id)
        {
            var book = GetBooks().FirstOrDefault(b => b.Id == id);
            if (book != null && book.IsAvailable)
            {
                var model = new BorrowModel { BookId = id };
                ViewBag.BookImageUrl = Url.Content("~/Content/Images/book.jpg"); // Pass the image URL to the view
                return View("BorrowForm", model);
            }

            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Borrow(BorrowModel model)
        {
            if (ModelState.IsValid)
            {
                // Handle form submission, e.g., save to database
                ViewBag.Message = "Book issued successfully!";
                return View("BorrowForm", model);
            }

            return View("BorrowForm", model);
        }
        public class Author
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string About { get; set; }
            public string ImageUrl { get; set; }
            public List<Book> Books { get; set; }
        }
        public ActionResult Authors()
        {
            var authors = GetAuthors();
            return View(authors);
        }

        // Mock data - replace with your data retrieval logic
        private List<Author> GetAuthors()
        {
            return new List<Author>
            {
                new Author { Id = 1, Name = "Ana Huang", About = "Ana Huang is an Amazon best-selling author of Young Adult and contemporary romance.", ImageUrl = Url.Content("~/Content/Images/author1.jpg"), Books = new List<Book>
                {
                    new Book { Id = 1, Title = "Twisted Love" },

                }},
                new Author { Id = 2, Name = "Colleen Hoover", About = "Colleen Hoover is an American author who primarily writes novels in the romance and young adult fiction genres.", ImageUrl = Url.Content("~/Content/Images/author2.jpg"), Books = new List<Book>
                {
                    new Book { Id = 2, Title = "It Ends with Us" },

                }},
                 new Author { Id = 3, Name = "Ali Ali Hazelwood", About = "Ali Hazelwood is the #1 New York Times bestselling author", ImageUrl = Url.Content("~/Content/Images/author3.jpg"), Books = new List<Book>
                {
                    new Book { Id = 3, Title = "Love on the Brain" },

                }},
            };
        }
    }


}
