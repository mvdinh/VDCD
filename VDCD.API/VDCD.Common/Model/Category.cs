using System;
using System.Collections.Generic;
using VDCD.Common.Base;

namespace VDCD.Common.Model
{
    public class Category : BaseEntity
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Navigation property
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
