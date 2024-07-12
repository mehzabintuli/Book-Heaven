using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public partial class Login
    {
        public int UserID { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
