using E_Ticaret.Context;
using E_Ticaret.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace E_Ticaret.Controllers
{
    public class CommentController : Controller
    {
        private ETicaretContext db = new ETicaretContext();

        //Yönetici kısmında sitedeki tüm yorumların listesi gelir.
        [Route("YorumlarTablosu")]
        public ActionResult ShowComments()
        {
            return View(db.Comments.Include("Product").Include("User").OrderByDescending(x => x.commentId));
        }

        //Ürün detay kısmındaki yorum formundan gelen içerik, yorum yapılan ürün ve yorumu yapan kullanıcı bilgileriyle birlikte veritabanına kaydedilir. 
        //Eğer oturum açılmamışsa yorum kaydedilmez ve giriş sayfasına yönlendirilir.
        [Route("YorumYap/{id:int}")]
        [HttpPost]
        public ActionResult AddComment(int id, Comment comment)
        {
            comment.ProductId = id;
            comment.UserId = Convert.ToInt32(Session["userId"]);
            if (Session["userId"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            comment.User = db.Users.Find(comment.UserId);
            comment.Product = db.Products.Find(comment.ProductId);
            db.Comments.Add(comment);
            db.SaveChanges();
            return RedirectToAction("Product","Home",id);
        }

        //Yorum sahibi, yorumunun yanındaki butonla yorumu silebilir veya yöneticiler istedikleri yorumu silebilir.
        [Route("YorumSil/{id:int}")]
        public ActionResult DeleteComment(int? id)
        {
            if(id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.FirstOrDefault(x=>x.commentId==id);
            if(comment==null)
            {
                return HttpNotFound();
            }

            db.Comments.Remove(comment);
            db.SaveChanges();
            if(Convert.ToInt32(Session["Authority"]) == 0)
            {
                return RedirectToAction("Index","Home");
            }
            return RedirectToAction("ShowComments");
        }
    }
}