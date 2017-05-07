using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC5Course.Models
{
    /// <summary>
    /// 這是一個精簡版的Product資料, 用於商品資料顯示
    /// </summary>
    public class ProductsListVM
    {
        public int ProductId { get; set; }
        [Required]
        [MinLength(5)]
        public string ProductName { get; set; }
        [Required]
        public Nullable<decimal> Price { get; set; }
        [Required]
        public Nullable<decimal> Stock { get; set; }
    }
}