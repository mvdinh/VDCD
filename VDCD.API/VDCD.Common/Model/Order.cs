using System;
using System.Collections.Generic;
using VDCD.Common.Base;

namespace VDCD.Common.Model
{
    public class Order : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Guid? CustomerId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalAmount { get; set; }
        public string PaymentMethod { get; set; } = "Tiền mặt";
        public string? Note { get; set; }

        // Navigation properties
        public virtual Customer? Customer { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
