using System;

namespace LibraryManagement.Models
{
    public class BorrowRequestModel
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime DueDate { get; set; }
    }
}
