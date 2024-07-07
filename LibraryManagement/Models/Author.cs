using System.Collections.Generic;

namespace LibraryManagement.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string About { get; set; }
        public string ImageUrl { get; set; }
        public virtual ICollection<Book> Books { get; set; } // Ensure Books property exists
    }
}
