//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LibraryManagement.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Author_Id { get; set; }
        public string Genre { get; set; }
        public string Language { get; set; }
        public bool Available { get; set; }
        public string Summary { get; set; }
        public string Cover_Image { get; set; }
        public string PDF_Link { get; set; }
    
        public virtual author author { get; set; }
    }
}
