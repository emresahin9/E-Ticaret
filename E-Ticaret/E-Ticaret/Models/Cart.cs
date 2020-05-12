using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace E_Ticaret.Models
{
    [Table("Sepet")]
    public class Cart
    {
        public Cart()
        {
            this.Products = new HashSet<Product>();
        }
        [ForeignKey("cartUser")]
        public int cartId { get; set; }
        public User cartUser { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}