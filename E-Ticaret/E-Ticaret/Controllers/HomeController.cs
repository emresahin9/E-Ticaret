using E_Ticaret.Context;
using E_Ticaret.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace E_Ticaret.Controllers
{
    public class HomeController : Controller
    {
        private ETicaretContext db = new ETicaretContext();

        //Sitenin anasayfası. Mevcut ürünler sayfalanarak ekrana gelir.
        [Route("AnaSayfa")]
        [Route("Home/Index")]
        [Route("")]
        public ActionResult Index(int page=1)
        {
            return View(db.Products.OrderByDescending(x=>x.categoryId).ToPagedList(page,10));
        }

        //Footerda iletişim bilgileri bulunur. 
        //Bu bilgiler yönetici tarafından güncellenebileceği için veritabanından çekilir ve partial view olarak layout ekranında bulunur ve her sayfada gösterilir.
        public ActionResult FooterPartial()
        {
            return PartialView(db.Contacts.Find(1));
        }

        //Kategoriler partialview olarak layout ekranında gösterilir ve her sayfada gösterilir. Böylelikle istenilen kategoriye kolayca erişilebilir.
        public ActionResult CategoryPartial()
        {
            return PartialView(db.Categories);
        }

        //Seçilen ürünün tüm bilgileri gösterilir. Aynı zamanda ürünün yorumları burada görülür ve yorum bu sayfada yapılır. Sepete ekleme yapılabilir.
        [Route("UrunGoster/{id:int}")]
        public ActionResult Product(int? id)
        {
            Product product = db.Products.Find(id);
            ViewBag.Kategori = db.Categories.FirstOrDefault(x => x.categoryId ==product.categoryId).categoryName;
            return View(product);
        }

        //Seçilen kategorideki tüm ürünler listelenir. Ürünün ayrıntılı bilgilerine ulaşılabilir veya ürün sepete eklenebilir.
        [Route("KategoriGoster/{id:int}")]
        public ActionResult CategoryPage(int? id)
        {
            ViewBag.Kategori = db.Categories.Find(id).categoryName;
            return View(db.Products.Include("productCategory").Where(x=>x.categoryId==id).ToList());
        }

        //Ürün detayının bulunduğu sayfada partial view olarak yorumlar gösterilir. Yorum sahibinin yorumunda yorum silme butonu bulunur.
        public ActionResult CommentPartial(int id)
        {
            return PartialView(db.Comments.Include("User").Where(x => x.ProductId == id));
        }
    }
}