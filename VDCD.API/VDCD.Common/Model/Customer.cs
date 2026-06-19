using System;
using System.Collections.Generic;
using VDCD.Common.Base;

namespace VDCD.Common.Model
{
    public class Customer : BaseEntity
    {
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Note { get; set; }

        // Navigation property
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
