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
using System.Net;

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


        public ActionResult AdminBook(int page = 1, int pageSize = 5)
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
            var book = new LibraryManagement.Models.book
            {
                Cover_Image = null,  // Explicitly set to null
                PDF_Link = null      // Explicitly set to null
            };

            var authors = db.authors.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name
            }).ToList();

            ViewBag.AuthorList = authors;

            return View(book);
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBook([Bind(Include = "Id,Title,Author_Id,Genre,Language,Available,Cover_Image,PDF_Link,Summary")] LibraryManagement.Models.book book, HttpPostedFileBase CoverImage, HttpPostedFileBase PDFFile)
        {
            if (ModelState.IsValid)
            {
                // Handle cover image upload
                if (CoverImage != null && CoverImage.ContentLength > 0)
                {
                    // Set the path for the uploaded image
                    string imageFileName = Path.GetFileName(CoverImage.FileName);
                    string imagePath = Path.Combine(Server.MapPath("~/Content/Images"), imageFileName);

                    // Save the image file to the server
                    CoverImage.SaveAs(imagePath);

                    // Update the Cover_Image field with the path to the new image
                    book.Cover_Image = "/Content/Images/" + imageFileName;
                }
                else
                {
                    // No new image uploaded, set Cover_Image to null or retain the old value
                    book.Cover_Image = null;
                }

                // Handle PDF file upload
                if (PDFFile != null && PDFFile.ContentLength > 0)
                {
                    // Set the path for the uploaded PDF
                    string pdfFileName = Path.GetFileName(PDFFile.FileName);
                    string pdfPath = Path.Combine(Server.MapPath("~/Content/PDFs"), pdfFileName);

                    // Save the PDF file to the server
                    PDFFile.SaveAs(pdfPath);

                    // Update the PDF_Link field with the path to the new PDF
                    book.PDF_Link = "/Content/PDFs/" + pdfFileName;
                }
                else
                {
                    // No new PDF uploaded, set PDF_Link to null or retain the old value
                    book.PDF_Link = null;
                }

                // Add the new book to the database
                db.books.Add(book);
                db.SaveChanges();
                return RedirectToAction("AdminBook");
            }

            // If model state is invalid, repopulate the author list and return to the view
            var authors = db.authors.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name
            }).ToList();

            ViewBag.AuthorList = authors;

            return View(book);
        }


        public ActionResult EditBook(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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

        // POST: Admin/EditBook
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBook([Bind(Include = "Id,Title,Author_Id,Genre,Language,Available,Cover_Image,PDF_Link,Summary")] book book, HttpPostedFileBase CoverImage, HttpPostedFileBase PDFFile)
        {
            string coverImage = db.books.Where(book_prev => book_prev.Id == book.Id)
                                .Select(book_prev => book_prev.Cover_Image)
                                .FirstOrDefault();
            string pdfLink = db.books.Where(book_prev => book_prev.Id == book.Id)
                                .Select(book_prev => book_prev.PDF_Link)
                                .FirstOrDefault();
            if (ModelState.IsValid)
            {
                // Handle cover image upload
                if (CoverImage != null && CoverImage.ContentLength > 0)
                {
                    // Set the path for the uploaded image
                    string imageFileName = Path.GetFileName(CoverImage.FileName);
                    string imagePath = Path.Combine(Server.MapPath("~/Content/Images"), imageFileName);

                    // Save the image file to the server
                    CoverImage.SaveAs(imagePath);

                    // Update the Cover_Image field with the path to the new image
                    book.Cover_Image = "/Content/Images/" + imageFileName;
                }
                else
                {
                    book.Cover_Image = coverImage;
                }

                // Handle PDF file upload
                if (PDFFile != null && PDFFile.ContentLength > 0)
                {
                    // Set the path for the uploaded PDF
                    string pdfFileName = Path.GetFileName(PDFFile.FileName);
                    string pdfPath = Path.Combine(Server.MapPath("~/Content/PDFs"), pdfFileName);

                    // Save the PDF file to the server
                    PDFFile.SaveAs(pdfPath);

                    // Update the PDF_Link field with the path to the new PDF
                    book.PDF_Link = "/Content/PDFs/" + pdfFileName;
                }
                else
                {
                    book.PDF_Link = pdfLink;
                }

                // Mark the entity as modified and save the changes
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AdminBook");
            }

            // If model state is invalid, repopulate the author list and return to the view
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
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(message))
            {
                // Add your email sending logic here (e.g., send an email to the user)
                System.Diagnostics.Debug.WriteLine("Reply sent to: " + email);
                System.Diagnostics.Debug.WriteLine("Message: " + message);

                // Return success message for AJAX
                return Json(new { success = true, message = "Reply sent successfully!" });
            }

            // Return failure message for AJAX
            return Json(new { success = false, message = "Error: Reply could not be sent." });
        }



        public ActionResult AdminReturnBook()
        {
            var returnBooks = db.ReturnBooks.ToList();
            return View(returnBooks);
        }




    }
}