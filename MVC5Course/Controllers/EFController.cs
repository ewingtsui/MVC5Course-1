using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC5Course.Models;
using System.Data.Entity.Validation;

namespace MVC5Course.Controllers
{
    public class EFController : Controller
    {
        FabricsEntities db = new FabricsEntities();

        // GET: EF
        public ActionResult Index()
        {
            var all = db.Product.AsQueryable();

            var data = all.Where(p => p.Active == true && p.ProductName.Contains("Black"))
                .OrderByDescending(p => p.ProductId);

            ////下面的型別都不同
            //var data1 = all.Where(p => p.ProductId == 1);
            //var data2 = all.FirstOrDefault(p => p.ProductId == 1);
            //var data3 = db.Product.Find(1);


            return View(data);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Product.Add(product);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Edit(int id)
        {
            var item = db.Product.Find(id);
            return View(item);
        }

        [HttpPost]
        public ActionResult Edit(int id, Product product)
        {
            if (ModelState.IsValid)
            {
                var item = db.Product.Find(id);
                item.ProductName = product.ProductName;
                item.Price = product.Price;
                item.Stock = product.Stock;
                item.Active = product.Active;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Delete(int? id)
        {
            var item = db.Product.Find(id);
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            var product = db.Product.Find(id);

            ////把product相關的OrderLine都一起抓出來
            //foreach(var item in product.OrderLine.ToList())
            //{
            //    db.OrderLine.Remove(item);
            //}

            ////這一句等於上面的那一句
            //db.OrderLine.RemoveRange(product.OrderLine);

            //db.Product.Remove(product);
            //在最後一步時才做SaveChange(), 以免有髒資料產生

            product.Is刪除 = true;

            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {

                throw ex;
            }
            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            var data = db.Database.SqlQuery<Product>("SELECT * FROM dbo.Product WHERE ProductID = @p0", id).FirstOrDefault();

            return View(data);
        }

        //刪除全部的資料
        public ActionResult RemoveAll()
        {
            ////效能很差
            //db.Product.RemoveRange(db.Product);
            //db.SaveChanges();

            //直接使用sql command, 效能最好
            db.Database.ExecuteSqlCommand("DELETE FROM dbo.Product");
            return View();
        }
    }
}