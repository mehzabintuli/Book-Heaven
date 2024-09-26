using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LibraryManagement.Controllers
{
    public class HomeController : Controller
    {
        BookHeavenEntities db = new BookHeavenEntities();


        public ActionResult Landing()
        {
            return View();
        }


        public ActionResult Admin()
        {

            return RedirectToAction("AdminBook", "Admin");
        }

        // Assuming this is inside your AccountController


        public new ActionResult User()
        {

            return View();
        }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }


        public ActionResult LearnMore()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            var userEmail = Session["UserEmail"] as string;
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Account"); // Redirect if user is not logged in
            }

            ViewBag.UserEmail = userEmail; // Pass the email to the view
            return View();
        }

        [HttpPost]
        public ActionResult Contact(string Subject, string Message)
        {
            var userEmail = Session["UserEmail"] as string;
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Account"); // Redirect if user is not logged in
            }

            if (ModelState.IsValid)
            {
                var contactMessage = new contact_messages
                {
                    email = userEmail, // Use email from session
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
            public string Author_Id { get; set; }
            public string Genre { get; set; }
            public string Language { get; set; }
            public bool Available { get; set; }
            public string Cover_Image { get; set; }
            public string PDF_Link { get; set; }
            public string Summary { get; set; }
        }

        private List<Book> GetBooks()
        {
            var books = db.books.Include(b => b.author).ToList();

            return books.Select(b => new Book
            {
                Id = b.Id,
                Title = b.Title,
                Author_Id = b.author.Name,
                Genre = b.Genre,
                Language = b.Language,
                Available = b.Available,
                Cover_Image = Url.Content(b.Cover_Image),
                PDF_Link = Url.Content(b.PDF_Link),
                Summary = b.Summary
            }).ToList();
        }




        public ActionResult Books(string searchString, int page = 1)
        {
            int pageSize = 5;
            var books = GetBooks();

            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.Title.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                         s.Author_Id.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
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
            if (book != null && book.Available)
            {
                var model = new BorrowBook { Id = id };
                ViewBag.BookImageUrl = Url.Content("~/Content/Images/book.jpg"); // Pass the image URL to the view
                return View("BorrowForm", model);
            }

            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult AddToList(int id)
        {
            var book = GetBooks().FirstOrDefault(b => b.Id == id);
            if (book != null && !book.Available)
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
                using (var db = new BookHeavenEntities())
                {
                    db.BorrowBooks.Add(model);
                    db.SaveChanges();
                }

                ViewBag.Message = "The book has been successfully borrowed!";
                return RedirectToAction("Books");
            }

            // If model state is not valid, redisplay the form with the current data
            return View("BorrowForm", model);
        }
        public ActionResult Policy()
        {
            return View();
        }
        public ActionResult Terms()
        {
            return View();
        }
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

        [HttpGet]
        public ActionResult EditUserAccount()
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserAccount(Signup model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Signups.FirstOrDefault(u => u.Id == model.Id);
                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Gender = model.Gender;
                    user.PhoneNumber = model.PhoneNumber;
                    user.DOB = model.DOB;
                    user.Password = model.Password;

                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("UserAccount");
                }
                return HttpNotFound();
            }

            return View(model);
        }




        public class Author
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string About { get; set; }
            public string Image { get; set; }
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
            var authors = db.authors.ToList();

            return authors.Select(a => new Author
            {
                Id = a.Id,
                Name = a.Name,
                About = a.About,
                Image = Url.Content(a.Image), // Assuming 'Image' in the database stores the path to the image
                Books = db.books.Where(b => b.Author_Id == a.Id).Select(b => new Book
                {
                    Id = b.Id,
                    Title = b.Title
                }).ToList()
            }).ToList();
        }


        [HttpGet]
        public ActionResult UserWishlist()
        {
            // Optionally initialize a new Wishlist object if needed
            var model = new Wishlist();


            ViewBag.BookImageUrl = Url.Content("~/Content/Images/slide5.jpg");

            return View(model);
        }

        [HttpPost]
        public ActionResult AddToWishlist(Wishlist model)
        {
            if (ModelState.IsValid)
            {
                db.Wishlists.Add(model);
                db.SaveChanges();

                ViewBag.Message = "Book added to your wishlist!";
                return RedirectToAction("UserWishlist"); // Redirect to the same view or any other appropriate view
            }


            return View("UserWishlist", model);
        }

        [HttpGet]
        public ActionResult ReturnBook()
        {
            ViewBag.BookImageUrl = Url.Content("~/Content/Images/index3.jpg"); // Placeholder image for return book form
            return View(new ReturnBook());
        }

        // POST: CreateReturnRequest
        [HttpPost]
        public ActionResult CreateReturnRequest(ReturnBook model)
        {
            if (ModelState.IsValid)
            {
                // Create a new entry for the ReturnBook table
                var returnBook = new ReturnBook
                {
                    Name = model.Name,
                    Email = model.Email,
                    ReturnDate = model.ReturnDate
                };

                // Add the new return record to the ReturnBook table
                db.ReturnBooks.Add(returnBook);
                db.SaveChanges(); // Save the changes to the database

                ViewBag.Message = "The book has been successfully returned!";
                return RedirectToAction("Books"); // Redirect to the "Books" action after success
            }

            // If model state is invalid, return the form with error messages
            return View("ReturnBook", model);
        }

        public ActionResult AuthorBooks()
        {
            // Fetch all records from the AuthorBook table
            var authorBooks = db.AuthorBooks.ToList();

            // Pass the data to the view
            return View(authorBooks);
        }
    }


}