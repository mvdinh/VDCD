using System;
using System.Collections.Generic;
using VDCD.Common.Base;

namespace VDCD.Common.Model
{
    public class Unit : BaseEntity
    {
        public Guid UnitId { get; set; }
        public string UnitName { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Navigation property
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
