using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace E_Ticaret.Models
{
    [Table("Kategori")]
    public class Category
    {
        [Key]
        public int categoryId { get; set; }
        [Required, MaxLength(30, ErrorMessage = "30 karakterden uzun olamaz."), DisplayName("Kategori Adı")]
        public string categoryName { get; set; }
        public IEnumerable<Product> categoryProduct { get; set; }
    }
}