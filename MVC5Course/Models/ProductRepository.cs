using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

namespace MVC5Course.Models
{
    public class ProductRepository : EFRepository<Product>, IProductRepository
    {
        public override IQueryable<Product> All()
        {
            return base.All().Where(p => !p.Is刪除);
        }

        public IQueryable<Product> All(bool showAll)
        {
            //若showAll為 true, 則連 Is刪除 = 0 的資料也會顯示出來, show base.All()
            if (showAll)
            {
                return base.All();
            }
            else  //若showAll為 false, 則 Is刪除 = 0 的資料不會顯示出來, show 上方override過的 this.All()
            {
                return this.All();
            }
        }

        public Product Get單筆資料ByProductId(int id)
        {
            return this.All().FirstOrDefault(p => p.ProductId == id);
        }

        public IQueryable<Product> GetProduct列表頁所有資料(bool Active, bool showAll = false)
        {
            IQueryable<Product> all = this.All();

            if (showAll)
            {
                all = base.All();
            }
            return all
                .Where(p => p.Active.HasValue && p.Active.Value == Active)
                .OrderByDescending(p => p.ProductId).Take(10);
        }

        public void Update(Product product)
        {
            this.UnitOfWork.Context.Entry(product).State = EntityState.Modified;
        }

        public override void Delete(Product entity)
        {
            //取消所有驗證
            this.UnitOfWork.Context.Configuration.ValidateOnSaveEnabled = false;

            entity.Is刪除 = true;

            //base.Delete(entity);
        }
    }

    public interface IProductRepository : IRepository<Product>
    {

    }
}