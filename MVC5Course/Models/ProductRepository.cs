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
            return base.All().Where(p => !p.Is�R��);
        }

        public IQueryable<Product> All(bool showAll)
        {
            //�YshowAll�� true, �h�s Is�R�� = 0 ����Ƥ]�|��ܥX��, show base.All()
            if (showAll)
            {
                return base.All();
            }
            else  //�YshowAll�� false, �h Is�R�� = 0 ����Ƥ��|��ܥX��, show �W��override�L�� this.All()
            {
                return this.All();
            }
        }

        public Product Get�浧���ByProductId(int id)
        {
            return this.All().FirstOrDefault(p => p.ProductId == id);
        }

        public IQueryable<Product> GetProduct�C���Ҧ����(bool Active, bool showAll = false)
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
            //�����Ҧ�����
            this.UnitOfWork.Context.Configuration.ValidateOnSaveEnabled = false;

            entity.Is�R�� = true;

            //base.Delete(entity);
        }
    }

    public interface IProductRepository : IRepository<Product>
    {

    }
}