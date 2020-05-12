using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using E_Ticaret.Context;
using E_Ticaret.Models;

namespace E_Ticaret.Controllers
{
    public class ContactsController : Controller
    {
        private ETicaretContext db = new ETicaretContext();

        //Yöneticiye, iletişim bilgilerinin değiştirileceği görünüm gelir.
        [Route("IletisimAdresleri/{id:int}")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        //Formdan alınan bilgilerle sitenin adres, eposta ve telefon bilgileri güncellenir.
        [Route("IletisimAdresleri/{id:int}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id,Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Result = "Bilgiler Güncellenmiştir";
            }
            else
                ViewBag.Result = "Hata";
            return View(contact);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
