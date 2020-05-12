using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace E_Ticaret.Models
{
    [Table("Yönetici")]
    public class Admin
    {
        [Key]
        public int adminId { get; set; }
        [Required,MaxLength(30,ErrorMessage ="30 karakterden uzun olamaz."),DisplayName("Ad Soyad")]
        public string adminFullname { get; set; }
        [Required, MaxLength(30, ErrorMessage = "30 karakterden uzun olamaz."),DisplayName("Eposta")]
        public string adminEmail { get; set; }
        [Required,DisplayName("Şifre")]
        public string adminPassword { get; set; }
    }
}