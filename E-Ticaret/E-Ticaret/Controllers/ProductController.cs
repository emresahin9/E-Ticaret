using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using E_Ticaret.Context;
using E_Ticaret.Models;

namespace E_Ticaret.Controllers
{
    public class ProductController : Controller
    {
        private ETicaretContext db = new ETicaretContext();

        //Yönetim bölümünde, tüm ürünler tablo şeklinde listelenir.
        [Route("UrunlerTablosu")]
        public ActionResult Index()
        {
            return View(db.Products.Include(p => p.productCategory).ToList().OrderBy(x => x.categoryId));
        }

        //Yönetim bölümünde, seçilen ürünün ayrıntılı bilgileri gösterilir.
        [Route("UrunAyrıntı/{id:int}")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Include("productCategory").FirstOrDefault(x=>x.productId==id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //Yeni ürün ekleneceği form, ekrana gelir.
        [Route("UrunEkle")]
        public ActionResult Create()
        {
            ViewBag.categoryId = new SelectList(db.Categories, "categoryId", "categoryName");
            return View();
        }

        //Formdan dönen bilgilerle yeni ürün veritabanına kaydedilir. Aynı zamanda resim urlsi kaydedilir ve Images klasörüne resimler kaydedilir.
        [Route("UrunEkle")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product,HttpPostedFileBase imageUrl)
        {
            if (ModelState.IsValid)
            {
                if (imageUrl != null)
                {
                    WebImage image = new WebImage(imageUrl.InputStream);
                    FileInfo info = new FileInfo(imageUrl.FileName);
                    image.Resize(400, 400);
                    string logoname = Guid.NewGuid().ToString() + info.Extension;
                    image.Save("~/Images/" + logoname);
                    product.productImageUrl = "/Images/" + logoname;
                }
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.categoryId = new SelectList(db.Categories, "categoryId", "categoryName", product.categoryId);
            return View(product);
        }

        //Seçili ürünün güncelleneceği form gelir.
        [Route("UrunGuncelle/{id:int}")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.categoryId = new SelectList(db.Categories, "categoryId", "categoryName", product.categoryId);
            return View(product);
        }

        //Formdan alınan bilgiler veritabanına kaydedilir. Kayıtlı bir resim varsa eski resim silinir ve yenisi eklenir. 
        [Route("UrunGuncelle/{id:int}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id,Product product, HttpPostedFileBase imageUrl)
        {
            if (ModelState.IsValid)
            {
                Product editproduct = db.Products.Find(id);
                if (imageUrl != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(editproduct.productImageUrl)))
                    { System.IO.File.Delete(Server.MapPath(editproduct.productImageUrl)); }
                    WebImage image = new WebImage(imageUrl.InputStream);
                    FileInfo info = new FileInfo(imageUrl.FileName);
                    image.Resize(400, 400);
                    string logoname = Guid.NewGuid().ToString() + info.Extension;
                    image.Save("~/Images/" + logoname);
                    editproduct.productImageUrl = "/Images/" + logoname;
                }
                editproduct.productName = product.productName;
                editproduct.productDescription = product.productDescription;
                editproduct.categoryId = product.categoryId;
                editproduct.price = product.price;
                editproduct.stock = product.stock;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.categoryId = new SelectList(db.Categories, "categoryId", "categoryName", product.categoryId);
            return View(product);
        }

        //Seçili ürün silinir.
        [Route("UrunSil/{id:int}")]
        public ActionResult Delete(int id)
        {
            Product product = db.Products.Find(id);
            if (System.IO.File.Exists(Server.MapPath(product.productImageUrl)))
            {
                System.IO.File.Delete(Server.MapPath(product.productImageUrl));
            }
            db.Products.Remove(product);
            db.SaveChanges();
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
