using System;

namespace VDCD.Common.Base
{
    public abstract class BaseEntity
    {
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
