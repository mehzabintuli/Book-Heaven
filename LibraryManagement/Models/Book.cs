namespace LibraryManagement.Models
{
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
}
