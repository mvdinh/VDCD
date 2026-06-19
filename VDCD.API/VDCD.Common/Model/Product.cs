using System;
using VDCD.Common.Base;

namespace VDCD.Common.Model
{
    public class Product : BaseEntity
    {
        [System.ComponentModel.DataAnnotations.Key]
        public Guid ProductId { get; set; }
        public Guid CategoryId { get; set; }
        [VDCD.Common.Attribute.Attribute.CheckDuplicate(ErrorMessage = "Tên sản phẩm đã tồn tại trong hệ thống.")]
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public Guid UnitId { get; set; }
        public string? Description { get; set; }

        // Navigation property
        public virtual Category? Category { get; set; }
        public virtual Unit? Unit { get; set; }
    }
}
