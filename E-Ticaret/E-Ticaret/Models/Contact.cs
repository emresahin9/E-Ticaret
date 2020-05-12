using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace E_Ticaret.Models
{
    [Table("İletişim")]
    public class Contact
    {
        [Key]
        public int contactId { get; set; }
        [Required, MaxLength(30, ErrorMessage = "30 karakterden uzun olamaz."), DisplayName("Eposta")]
        public string contactEmail { get; set; }
        [Required, MaxLength(200, ErrorMessage = "200 karakterden uzun olamaz."), DisplayName("Adres")]
        public string contactAdress { get; set; }
        [Required, MaxLength(20, ErrorMessage = "20 karakterden uzun olamaz."), DisplayName("Telefon Numarası")]
        public string contactTel { get; set; }
    }
}