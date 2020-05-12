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
using System.Web.Helpers;
using PagedList;
using PagedList.Mvc;

namespace E_Ticaret.Controllers
{
    public class CategoryController : Controller
    {
        private ETicaretContext db = new ETicaretContext();

        //Veritabanındaki kategoriler ekrana gelir.
        [Route("KategorilerTablosu")]
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        //Yeni kategori ekleneceği ekran gelir
        [Route("KategoriEkle")]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        //Form doldurulur ve yeni kategori eklenir.
        [Route("KategoriEkle")]
        [HttpPost]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

        //Mevcut bir kategorinin güncelleneceği ekran gelir.
        [Route("KategoriGuncelle/{id:int}")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        //Form bilgileri alınır ve kategori bilgileri güncellenir.
        [Route("KategoriGuncelle/{id:int}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        //Seçili kategori silinir. 
        //Aynı zamanda, Category tablosuna bağlı olan Product tablosundaki ürün verileri ve aynı şekilde Product tablosuna bağlı olan Comment tablosundaki yorum verileri de silinir.
        [Route("KategoriSil/{id:int}")]
        public ActionResult Delete(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            IEnumerable<Product> productToBeDeleted = db.Products.Where(x => x.categoryId == category.categoryId);
            foreach(Product product in productToBeDeleted)
            {
                System.IO.File.Delete(Server.MapPath(product.productImageUrl));
                db.Products.Remove(product);
                db.SaveChanges();
                IEnumerable<Comment> commentToBeDeleted = db.Comments.Where(x => x.ProductId == product.productId);
                foreach (Comment comment in commentToBeDeleted)
                {
                    db.Comments.Remove(comment);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
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
