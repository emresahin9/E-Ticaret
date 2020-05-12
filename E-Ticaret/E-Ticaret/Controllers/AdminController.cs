using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using E_Ticaret.Context;
using E_Ticaret.Models;

namespace E_Ticaret.Controllers
{
    public class AdminController : Controller
    {
        private ETicaretContext db = new ETicaretContext();

        //Yöneticinin profilindeki değişikliklerin yapıldığı ekran gelir.
        [Route("YonetimProfil")]
        public ActionResult Profile()
        {
            Admin profile = db.Admins.Find(Session["Id"]);
            if (profile == null)
            {
                return RedirectToAction("Index");
            }
            return View(profile);
        }

        //Formdan gelen bilgiler kontrol edilir. Eski şifre kontrol edilir, eğer doğruysa yeni şifre ,şifrelenerek kaydedilir ve diğer bilgiler de kaydedilir.
        [Route("YonetimProfil")]
        [HttpPost]
        public ActionResult Profile(Admin admin,string adminPasswordnew)
        {
            Admin profile = db.Admins.Find(Session["Id"]);
            if (profile == null)
            {
                return RedirectToAction("Index");
            }
            if (profile.adminPassword == Crypto.Hash(admin.adminPassword, "MD5"))
            {
                profile.adminPassword = Crypto.Hash(adminPasswordnew, "MD5");
                profile.adminEmail = admin.adminEmail;
                profile.adminFullname = admin.adminFullname;
                db.SaveChanges();
                ViewBag.result = "Değişiklikler Kaydedilmiştir";
                return RedirectToAction("Index");
            }
            ViewBag.result = "Değişiklikler Kaydedilmemiştir";
            return View(profile);
        }

        //Mevcut yönetici hesaplarının ekrana listelenir.
        [Route("YonetimPaneli")]
        public ActionResult Index()
        {
            return View(db.Admins.ToList());
        }

        //Yöneticinin giriş yapacağı ekranın gelir.
        [Route("YonetimGiris")]
        public ActionResult Login()
        {
            return View();
        }

        //Giriş bilgileri alınır, kontrol edilir ve bilgiler doğruysa Session açıp kullanıcı bilgileri burada tutulur.
        [Route("YonetimGiris")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Admin admin)
        {
            if(admin==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin loginuser = db.Admins.FirstOrDefault(x => x.adminEmail == admin.adminEmail);
            if(loginuser!=null)
            {
                admin.adminPassword = Crypto.Hash(admin.adminPassword, "MD5");
                if (admin.adminPassword == loginuser.adminPassword)
                {
                    Session["Email"] = loginuser.adminEmail;
                    Session["FullName"] = loginuser.adminFullname;
                    Session["Id"] = loginuser.adminId;
                    Session["authority"] = 1;
                    return RedirectToAction("Profile");
                }
                ViewBag.error = "Email veya şifre yanlış";
            }
            return View();
        }

        //Yönetici hesabından çıkış yapılır ve Session sonlandırılır.
        [Route("YonetimCikis")]
        public ActionResult Logout()
        {
            Session["Email"] = null;
            Session["FullName"] = null;
            Session["Id"] = null;
            Session["authority"] = null;
            Session.Abandon();
            return RedirectToAction("Login");
        }

        //Şifresini unutan yöneticinin email ile şifresini alacağı form gelir.
        [Route("YonetimSifremiUnuttum")]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        
        //Girilen formdaki eposta adresi kontrol edilir.Eposta adresi kayıtlı değilse hata döndürülür.
        //Kayıtlıysa sitenin mail adresinden yöneticiye yeni bir şifre gönderilir ve bu şifre MD5 ile şifrelenerek kaydedilir.
        [Route("YonetimSifremiUnuttum")]
        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            if(email==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.FirstOrDefault(x => x.adminEmail == email);
            if (admin == null)
            {
                ViewBag.Sonuc = "Böyle bir eposta adresi bulunamadı!";
            }
            else
            {
                Random rnd = new Random();
                var number = rnd.Next();
                var newPassword = Crypto.Hash(number.ToString(), "MD5");
                admin.adminPassword = newPassword;
                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.EnableSsl = true;
                WebMail.UserName = "abbadabaka@gmail.com";
                WebMail.Password = "asdfgh1346";
                WebMail.SmtpPort = 587;
                try
                {
                    WebMail.Send(
                        to: email, subject: "Şifre Yenileme", body:"Şifreniz: "+ number.ToString(), from: "abbadabaka@gmail.com");
                    ViewBag.Sonuc = "Yeni şifre Eposta adresinize gönderilmiştir.";
                    db.SaveChanges();
                }
                catch (Exception e)
                { ViewBag.Sonuc = e.Message; }
            }
            return View();
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
