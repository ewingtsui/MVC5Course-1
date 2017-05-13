using System;
using System.Linq;
using System.Collections.Generic;
	
namespace MVC5Course.Models
{   
	public  class ProductRepository : EFRepository<Product>, IProductRepository
    {
        
         public Product Get³æµ§¸ê®ÆByProductId(int id)
         {
             return this.All().FirstOrDefault(p => p.ProductId == id);
         }
}

	public  interface IProductRepository : IRepository<Product>
	{

	}
}