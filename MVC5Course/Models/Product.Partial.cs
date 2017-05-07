namespace MVC5Course.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using ValidationAttributes;

    [MetadataType(typeof(ProductMetaData))]
    public partial class Product : IValidatableObject
    {
        public int 訂單數量
        {
            get
            {
                //return this.OrderLine.Count;

                //return this.OrderLine.Where(p => p.Qty > 100).Count();
                //return this.OrderLine.Where(p => p.Qty > 100).ToList().Count();
                return this.OrderLine.Count(p => p.Qty > 0); //這個方法效能最好, 上面3個效能很差
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.Price > 100 && this.Stock > 5)
            {
                yield return new ValidationResult("價格與庫存數量不合理", new string[] { "Price", "Stock" });
            }

            if (this.OrderLine.Count() > 5 && this.Stock == 0)
            {
                yield return new ValidationResult("Stock 與訂單數量不匹配",new string[] { "Stock" });
            }

            yield break;
        }
    }

    public partial class ProductMetaData
    {
        [Required]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "請輸入產品名稱")]
        [MinLength(3), MaxLength(300)]
        //[RegularExpression("(.+)-(.+)", ErrorMessage = "商品名稱錯誤")]
        [DisplayName("商品名稱")]
        [商品名稱必須包含Ewing字串(ErrorMessage = "商品名稱未包含Ewing字串")]
        public string ProductName { get; set; }

        [Required]
        [Range(0, 9999999, ErrorMessage = "請設定正確的商品價格範圍")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0}")]
        [DisplayName("商品價格")]
        public Nullable<decimal> Price { get; set; }

        [Required]
        [DisplayName("是否上架")]
        public Nullable<bool> Active { get; set; }

        [Required]
        [Range(0, 2000, ErrorMessage = "請設定正確的庫存數量範圍")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0}")]
        [DisplayName("庫存數量")]
        public Nullable<decimal> Stock { get; set; }

        public virtual ICollection<OrderLine> OrderLine { get; set; }
    }
}
