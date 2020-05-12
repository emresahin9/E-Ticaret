using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace E_Ticaret.Models
{
    [Table("Ürün")]
    public class Product
    {
        public Product()
        {
            this.Carts = new HashSet<Cart>();
        }
        [Key]
        public int productId { get; set; }
        [Required, MaxLength(30, ErrorMessage = "30 karakterden uzun olamaz."), DisplayName("Ürün Adı")]
        public string productName { get; set; }
        [Required, DisplayName("Ürün Açıklaması")]
        public string productDescription { get; set; }
        [Required]
        public int categoryId { get; set; }
        public Category productCategory { get; set; }
        public string productImageUrl { get; set; }
        [Required,DisplayName("Stok Adedi")]
        public int? stock { get; set; }
        [Required, DisplayName("Ürün Fiyatı")]
        public double? price { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public IEnumerable<Comment> productComments { get; set; }
    }
}