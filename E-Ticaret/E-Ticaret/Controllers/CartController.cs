using E_Ticaret.Context;
using E_Ticaret.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_Ticaret.Controllers
{
    public class CartController : Controller
    {
        private ETicaretContext db = new ETicaretContext();

        //Kullanıcının seçilen ürününü, kullanıcının sepetine ekler.
        [Route("SepeteEkle/{id:int}")]
        public ActionResult AddtoCart(int id)
        {
            if (Session["userId"] == null)
            {
                return RedirectToAction("Login","User");
            }
            int thisUserId = Convert.ToInt32(Session["userId"]);
            Product thisProduct = db.Products.Find(id);
            if (db.Carts.FirstOrDefault(x => x.cartUser.userId == thisUserId) == null)
            {
                Cart cart = new Cart();
                db.Users.Find(thisUserId).cart = cart;
                db.SaveChanges();
            }
            db.Carts.Include("cartUser").FirstOrDefault(x => x.cartUser.userId == thisUserId).Products.Add(thisProduct);
            db.SaveChanges();
            return RedirectToAction("Cart");
        }

        //Kullanıcının sepetteki seçtiği ürünü sepetten çıkartır.
        [Route("SepettenCikar/{id:int}")]
        public ActionResult RemovingFromCart(int id)
        {
            int thisUserId = Convert.ToInt32(Session["userId"]);
            Product thisProduct = db.Products.Find(id);
            db.Carts.Include("cartUser").FirstOrDefault(x => x.cartUser.userId == thisUserId).Products.Remove(thisProduct);
            db.SaveChanges();
            return RedirectToAction("Cart");
        }

        //Kullanıcın sepet sayfasını getirir.
        [Route("Sepet")]
        public ActionResult Cart()
        {
            int thisuserId = Convert.ToInt32(Session["userId"]);
            User thisuser = db.Users.Include("cart").FirstOrDefault(x => x.userId == thisuserId);
            return View(db.Carts.FirstOrDefault(x => x.cartUser.userId == thisuserId).Products.ToList());
        }

        //Kullanıcnın sepetteki ürünlerini satın alır. Bu ürünlerin stokları eksilir.
        [Route("SatinAl/{id:int}")]
        public ActionResult Buy(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Cart");
            }
            Cart cart = db.Carts.Include("Products").FirstOrDefault(x => x.cartId == id);
            foreach (var i in cart.Products)
            {
                i.stock--;
            }
            cart.Products.Clear();
            db.SaveChanges();
            return RedirectToAction("Cart");
        }
    }
}