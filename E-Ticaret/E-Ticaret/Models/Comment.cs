using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace E_Ticaret.Models
{
    [Table("Yorum")]
    public class Comment
    {
        [Key]
        public int commentId { get; set; }
        [Required, MaxLength(500, ErrorMessage = "500 karakterden uzun olamaz."), DisplayName("Yorum İçeriği")]
        public string commentContent { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}