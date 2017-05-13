using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5Course.Models;

namespace MVC5Course.Controllers
{
    public class ProductsController : baseController
    {
        ProductRepository repo = RepositoryHelper.GetProductRepository();

        //private FabricsEntities db = new FabricsEntities();

        // GET: Products
        public ActionResult Index(bool Active1 = true)
        {
            //最新寫法
            var data = repo.GetProduct列表頁所有資料(Active1, showAll: false);

            ////使用 Repository寫法
            //var data = repo.All()
            //    .Where(d => d.Active.HasValue && d.Active.Value == Active1)
            //    .OrderByDescending(p => p.ProductId).Take(10);

            ////原來寫法
            //var data = db.Product
            //    .Where(d => d.Active.HasValue && d.Active.Value == Active1)
            //    .OrderByDescending(p => p.ProductId).Take(10);
            return View(data);
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //使用 ProductRepository 的寫法
            Product product = repo.Get單筆資料ByProductId(id.Value);
            //Product product = db.Product.Find(id);  --原來寫法
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,ProductName,Price,Active,Stock")] Product product)
        {
            if (ModelState.IsValid)
            {
                //Repository
                repo.Add(product);
                repo.UnitOfWork.Commit();

                ////原來寫法
                //db.Product.Add(product);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Repository
            Product product = repo.Get單筆資料ByProductId(id.Value);

            ////原來寫法
            //Product product = db.Product.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,ProductName,Price,Active,Stock")] Product product)
        {
            if (ModelState.IsValid)
            {
                //Repository
                repo.Update(product);
                repo.UnitOfWork.Commit();

                ////原來寫法
                //db.Entry(product).State = EntityState.Modified;
                //db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Repository
            Product product = repo.Get單筆資料ByProductId(id.Value);

            ////原來寫法
            //Product product = db.Product.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = repo.Get單筆資料ByProductId(id);

            ////取消所有驗證 (也可以放在Repository)
            //repo.UnitOfWork.Context.Configuration.ValidateOnSaveEnabled = false;

            //Repository
            repo.Delete(product);

            //刪除Product之前先將OrderLine中同樣ProductId的資料也一併刪除
            //var repoOrderLines = 
            //    RepositoryHelper.GetOrderLineRepository(repo.UnitOfWork);  //這種寫法可以共用repo的unitOfWork的commit, 不用另外再寫一個commit

            //foreach (var item in product.OrderLine)
            //{
            //    repoOrderLines.Delete(item);
            //}

            repo.UnitOfWork.Commit();

            ////原來寫法
            //Product product = db.Product.Find(id);
            //db.Product.Remove(product);
            //db.SaveChanges();

            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}


        public ActionResult ListProducts()
        {
            var data = repo.GetProduct列表頁所有資料(true)
                 .Select(p => new ProductsListVM()
                 {
                     ProductId = p.ProductId,
                     ProductName = p.ProductName,
                     Price = p.Price,
                     Stock = p.Stock
                 });

            //    var data = db.Product
            //        .Where(d => d.Active.HasValue && d.Active == true)
            //        .Select(d => new ProductsListVM()
            //        {
            //            ProductId = d.ProductId,
            //            ProductName = d.ProductName,
            //            Price = d.Price,
            //            Stock = d.Stock
            //        });

            return View(data);
        }

        public ActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateProduct([Bind(Include = "ProductId,ProductName,Price,Active,Stock")] ProductsListVM data)
        {
            if (ModelState.IsValid)
            {
                //db.Product.Add(data);
                //db.SaveChanges();
                return RedirectToAction("ListProducts");
            }
            return View();
        }
    }
}
