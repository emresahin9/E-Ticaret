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
    public class UserController : Controller
    {
        private ETicaretContext db = new ETicaretContext();

        //Yonetim bölümünde, kayıtlı olan tüm kullanıcılar tablo şeklinde listelenir.
        [Route("KayitliKullanicilar")]
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        //Kullanıcı kayıt olacağı form ekranı gelir.
        [Route("KullaniciKayit")]
        public ActionResult SignIn()
        {
            return View();
        }

        //Formdan gelen bilgilerle yeni kullanıcı oluşturulur. Aynı eposta adresiyle kayıtlı bir hesap varsa kaydedilmez. Kullanıcının girdiği şifre MD5 ile şifrelenir.
        [Route("KullaniciKayit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(User user)
        {
            if (ModelState.IsValid)
            {
                if(db.Users.FirstOrDefault(x=>x.userEmail==user.userEmail)!=null)
                {
                    ViewBag.result = "Email adresi zaten kayıtlı!";
                    return View();
                }
                user.userPassword = Crypto.Hash(user.userPassword, "MD5");
                db.Users.Add(user);
                db.SaveChanges();
                ViewBag.result = "Başarıyla kayıt oldunuz";
                return View();
            }
            ViewBag.result = "Lütfen tüm alanları doldurunuz";
            return View();
        }

        //Kullanıcı giriş formu gelir.
        [Route("KullaniciGiris")]
        public ActionResult Login()
        {
            return View();
        }

        //Giriş formunda gelen bilgiler kontrol edilir. Eposta adresi kayıtlı değilse uyarı döndürülür, şifre yanlışsa da uyarı döndürülür. 
        //Bilgiler doğruysa Session açılıp kullanıcı bilgileri burada tutulur ve anasayfaya yönlendirilir.
        [Route("KullaniciGiris")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            if(user==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User loginUser = db.Users.FirstOrDefault(x => x.userEmail == user.userEmail);
            if (loginUser==null)
            {
                ViewBag.error = "Böyle bir kullanıcı kayıtlı değil";
                return View();
            }
            if (loginUser.userPassword == Crypto.Hash(user.userPassword, "MD5"))
            {
                Session["userEmail"] = loginUser.userEmail;
                Session["userName"] = loginUser.userFullname;
                Session["userId"] = loginUser.userId;
                Session["authority"] = 0;
                if (db.Carts.FirstOrDefault(x => x.cartUser.userId == loginUser.userId) == null)
                {
                    Cart cart = new Cart();
                    loginUser.cart = cart;
                    db.SaveChanges();
                }
                Session["userCartId"] = loginUser.cart.cartId;
                return RedirectToAction("Index", "Home");
            }
            ViewBag.error = "Yanlış Şifre";
            return View();
        }

        //Kullanıcı oturumu sonlandırılır. Session bilgileri silinir ve giriş ekranına yönlendirilir.
        [Route("Cikis")]
        public ActionResult Logout()
        {
            Session["userEmail"] = null;
            Session["userName"] = null;
            Session["userId"] = null;
            Session["userCartId"] = null;
            Session["authority"] = null;
            Session.Abandon();
            return RedirectToAction("Login");
        }

        //Kullanıcıya eposta adresinin istendiği form gelir. 
        [Route("SifremiUnuttum")]
        public ActionResult UserForgotPassword()
        {
            return View();
        }

        //Girilen formdaki eposta adresi kontrol edilir. Eposta adresi kayıtlı değilse hata döndürülür. 
        //Kayıtlıysa sitenin mail adresinden kullanıcıya yeni bir şifre gönderilir ve bu şifre MD5 ile şifrelenerek kaydedilir.
        [Route("SifremiUnuttumm")]
        [HttpPost]
        public ActionResult UserForgotpassword(string email)
        {
            if (email == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.FirstOrDefault(x => x.userEmail == email);
            if (user == null)
            {
                ViewBag.Sonuc = "Böyle bir eposta adresi bulunamadı!";
            }
            else
            {
                Random rnd = new Random();
                var number = rnd.Next();
                var newPassword = Crypto.Hash(number.ToString(), "MD5");
                user.userPassword = newPassword;
                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.EnableSsl = true;
                WebMail.UserName = "abbadabaka@gmail.com";
                WebMail.Password = "asdfgh1346";
                WebMail.SmtpPort = 587;
                try
                {
                    WebMail.Send(
                        to: email, subject: "Şifre Yenileme", body: "Şifreniz: " + number.ToString(), from: "abbadabaka@gmail.com");
                    ViewBag.Sonuc = "Yeni şifre Eposta adresinize gönderilmiştir.";
                    db.SaveChanges();
                }
                catch (Exception e)
                { ViewBag.Sonuc = e.Message; }
            }
            return View();
        }

        //Kullanıcı bilgilerinin düzenleneceği form gelir.
        [Route("KullanıcıBilgileriDüzenle/{id:int}")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //Formdan gelen bilgiler kontrol edilir. Eski şifre yanlışsa hata döndürülür. Doğruysa yeni şifre MD5 ile şifrelenerek kaydedilir.
        [Route("KullanıcıBilgileriDüzenle/{id:int}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user,string newPassword)
        {
            if (ModelState.IsValid)
            {
                User newUser = db.Users.Find(user.userId);
                if (newUser.userPassword == Crypto.Hash(user.userPassword, "MD5"))
                {
                    newUser.userFullname = user.userFullname;
                    newUser.userEmail = user.userEmail;
                    newUser.userPassword = Crypto.Hash(newPassword, "MD5");
                    ViewBag.Result = "Değişiklikler Kaydedilmiştir";
                    db.SaveChanges();
                    return View(user);
                }
                ViewBag.Result = "Şifreniz Yanlış";
                return View(user);
            }
            return View(user);
        }

        //Kullanıcı silinir ve oturum sonlandırılır. Session bilgileri sıfırlanır.
        [Route("KullaniciSil/{id:int}")]
        public ActionResult Delete(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            Cart deleteCart = db.Carts.Include("cartUser").FirstOrDefault(x => x.cartUser.userId == id);
            db.Carts.Remove(deleteCart);
            db.SaveChanges();
            Session["userEmail"] = null;
            Session["userName"] = null;
            Session["userId"] = null;
            Session["userCartId"] = null;
            Session.Abandon();
            return RedirectToAction("Login");
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
