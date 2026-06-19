using System;
using VDCD.Common.Base;

namespace VDCD.Common.Model
{
    public class Product : BaseEntity
    {
        public Guid ProductId { get; set; }
        public Guid CategoryId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? SKU { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; } = "Cái";
        public string? Description { get; set; }

        // Navigation property
        public virtual Category? Category { get; set; }
    }
}
