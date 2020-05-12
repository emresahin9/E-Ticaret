using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace E_Ticaret.Models
{
    [Table("Kullanıcı")]
    public class User
    {
        [Key]
        public int userId { get; set; }
        [Required, MaxLength(30, ErrorMessage = "30 karakterden uzun olamaz."), DisplayName("Ad Soyad")]
        public string userFullname { get; set; }
        [Required, MaxLength(30, ErrorMessage = "30 karakterden uzun olamaz."), DisplayName("Eposta")]
        public string userEmail { get; set; }
        [Required, DisplayName("Şifre")]
        public string userPassword { get; set; }
        public IEnumerator<Comment> userComment { get; set; }
        public virtual Cart cart { get; set; }
    }
}