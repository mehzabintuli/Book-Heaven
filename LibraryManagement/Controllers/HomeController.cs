using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryManagement.Controllers
{
    public class HomeController : Controller
    {

        

        BookHeavenEntities db = new BookHeavenEntities();
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

        public new ActionResult User()
        {
            // Redirect to User page or view
            //return RedirectToAction("UserDashboard", "User");
            return View();
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
        public ActionResult Contact(string Email, string Subject, string Message)
        {
            if (ModelState.IsValid)
            {
                var contactMessage = new contact_messages
                {
                    email = Email,
                    subject = Subject,
                    message = Message,
                    created_at = DateTime.Now
                };

                db.contact_messages.Add(contactMessage);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Thank you for contacting us! We will get back to you soon.";
                return RedirectToAction("Contact");
            }
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
            public string Summary { get; set; }
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
                     new Book { Id = 1, Title = "Twisted Love", Author = "Ana Huang", Genre = "Romance Novel", Language = "English", IsAvailable = true, ImageUrl = Url.Content("~/Content/Images/book1.jpg"), PdfUrl = Url.Content("~/Content/Pdfs/book1.pdf"), Summary = "Alex is a brooding, complex man with a dark past, while Ava is a warm-hearted woman determined to melt his icy exterior.As their worlds collide, love blooms amidst secrets and shadows. Will their love survive the twists of fate?" },
                     new Book { Id = 2, Title = "It Ends with Us", Author = "Colleen Hoover", Genre = "Contemporary Romance", Language = "English", IsAvailable = true, ImageUrl = Url.Content("~/Content/Images/book2.jpg"), PdfUrl = Url.Content("~/Content/Pdfs/book2.pdf"), Summary = "Lily's life seems perfect when she meets the charming neurosurgeon Ryle, but old wounds resurface when her first love, Atlas, reappears.Torn between past and present, she must make a heartbreaking decision. In the end, love’s true strength lies in letting go." },
                     new Book { Id = 3, Title = "Love on the Brain", Author = "Ali Hazelwood", Genre = "Romance Novel", Language = "English", IsAvailable = false, ImageUrl = Url.Content("~/Content/Images/book3.jpg"), PdfUrl = Url.Content("~/Content/Pdfs/book3.pdf"), Summary = "Neuroscientist Bee Königswasser is forced to collaborate with her archenemy, Levi Ward, on a NASA project. As they navigate scientific challenges and personal misunderstandings, sparks fly. Sometimes love is just a neuron away." },
                     new Book { Id = 4, Title = "The Housemaid", Author = "Freida McFadden", Genre = "Thriller", Language = "English", IsAvailable = true, ImageUrl = Url.Content("~/Content/Images/book4.jpg"), PdfUrl = Url.Content("~/Content/Pdfs/book4.pdf"), Summary = "Millie takes a job as a housemaid for the wealthy but eccentric Winchester family. As she uncovers the household's dark secrets, she realizes she's in over her head. Can she escape before the house consumes her?" },
                     new Book { Id = 5, Title = "Harry Potter and the Philosopher's Stone", Author = "J. K. Rowling", Genre = "Fantasy Fiction", Language = "English", IsAvailable = false, ImageUrl = Url.Content("~/Content/Images/book5.jpg"), PdfUrl = Url.Content("~/Content/Pdfs/book5.pdf"), Summary = "It is a story about Harry Potter, an orphan brought up by his aunt and uncle because his parents were killed when he was a baby but everything changes when he is invited to join Hogwarts School of Witchcraft and Wizardry and he finds out he's a wizard." },
                     new Book { Id = 6, Title = "Murder on the Orient Express", Author = "Agatha Christie", Genre = "Detective Fiction", Language = "English", IsAvailable = true, ImageUrl = Url.Content("~/Content/Images/book6.jpg"), PdfUrl = Url.Content("~/Content/Pdfs/book6.pdf"), Summary = "Detective Hercule Poirot's luxurious train journey turns into a thrilling mystery when a passenger is found murdered.With the train stranded in a snowstorm, everyone is a suspect. Poirot must use his keen intellect to unravel the intricate web of lies and deceit." }
                 };
        }


        public ActionResult Books(string searchString, int page = 1)
        {
            int pageSize = 3;
            var books = GetBooks();

            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.Title.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                         s.Author.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }

            var paginatedBooks = books.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)books.Count / pageSize);
            ViewBag.SearchString = searchString;

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
        [HttpGet]
        public ActionResult AddToList(int id)
        {
            var book = GetBooks().FirstOrDefault(b => b.Id == id);
            if (book != null && !book.IsAvailable)
            {
                // Add logic to add the book to the user's list
                // For example, save to the database or session
                ViewBag.Message = "Book added to your list!";
                return RedirectToAction("Books");
            }

            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult CreateBorrowRequest(BorrowBook model)
        {
            if (ModelState.IsValid)
            {
                AdminController adminController = new AdminController();
                adminController.AddBorrowRequest(model);

                // Additional logic if needed
                return RedirectToAction("Index");
            }

            return View(model);
        }
        //public ActionResult UserAccount()
        //{
        //    return View();
        //}

        public ActionResult UserAccount()
        {
            var userEmail = Session["UserEmail"] as string;
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Account");
            }

            var user = db.Signups.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        public class Author
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string About { get; set; }
            public string ImageUrl { get; set; }
            public List<Book> Books { get; set; }
        }

        public ActionResult Authors(int page = 1)
        {
            int pageSize = 4;
            var authors = GetAuthors();

            var paginatedAuthors = authors.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)authors.Count / pageSize);

            return View(paginatedAuthors);
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
                new Author { Id = 3, Name = "Ali Hazelwood", About = "Ali Hazelwood is the #1 New York Times bestselling author", ImageUrl = Url.Content("~/Content/Images/author3.jpg"), Books = new List<Book>
                {
                    new Book { Id = 3, Title = "Love on the Brain" },
                }},
                new Author { Id = 4, Name = "Freida McFadden", About = "Freida McFadden is the pen name of an American thriller author and practicing physician specializing in brain injury.", ImageUrl = Url.Content("~/Content/Images/author4.0.jpg"), Books = new List<Book>
                {
                    new Book { Id = 4, Title = "The Housemaid" },
                }},
                new Author { Id = 5, Name = "J. K. Rowling", About = "Joanne Rowling CH OBE FRSL, known by her pen name J. K. Rowling, is a British author and philanthropist.", ImageUrl = Url.Content("~/Content/Images/author4.jpg"), Books = new List<Book>
                {
                    new Book { Id = 5, Title = "Harry Potter and the Philosopher's Stone" },
                }},
                new Author { Id = 6, Name = "Agatha Christie", About = "Dame Agatha Mary Clarissa Christie was an English writer known for her 66 detective novels and 14 short story collections.", ImageUrl = Url.Content("~/Content/Images/author5.jpg"), Books = new List<Book>
                {
                    new Book { Id = 6, Title = "Murder on the Orient Express" },
                }},
            };
        }
    }


}
