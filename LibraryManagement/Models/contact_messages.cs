namespace LibraryManagement.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class contact_messages
    {
        public int id { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        public string subject { get; set; }

        [Required]
        public string message { get; set; }

        [Required]
        public DateTime created_at { get; set; } = DateTime.Now;
    }
}
