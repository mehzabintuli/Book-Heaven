using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LibraryManagement.Models;

namespace LibraryManagement.Controllers
{
    public class BooksController : Controller
    {
        BookHeavenEntities db = new BookHeavenEntities();
        // Sample data
        private List<book> GetBooks()
        {
            return db.books.ToList(); 
            //return new List<LibraryBook>
            //{
            //    new LibraryBook { Id = 1, Title = "Teach Yourself C", Author = "Herbert Schildt", ImageUrl = "~/Content/Images/c.jpg" },
            //    new LibraryBook { Id = 2, Title = "Shatter Me", Author = "Tahereh Mafi", ImageUrl = "~/Content/Images/book7.0.jpg" },
            //    new LibraryBook { Id = 3, Title = "King of Sloth", Author = "Ana Huang", ImageUrl = "~/Content/Images/book9.jpg" },
            //    new LibraryBook { Id = 4, Title = "Quantum mechanics", Author = "George Hrabovsky and Leonard Susskind", ImageUrl = "~/Content/Images/book10.jpg" },
            //    new LibraryBook { Id = 5, Title = "The Maddest Obsession", Author = "Danielle Lori", ImageUrl = "~/Content/Images/book8.jpg" }
            //};
        }


        public ActionResult BooksList(string searchQuery)
        {
            var books = GetBooks();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                //books = books.Where(b => b.Title.Contains(searchQuery) || b.Author.Contains(searchQuery)).ToList();
                books = books.Where(b => b.Title.Contains(searchQuery)).ToList();
            }

            return View(books);
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            var book = GetBooks().FirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                return HttpNotFound();
            }

            return View(book);
        }
    }
}
