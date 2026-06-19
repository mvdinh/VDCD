using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDCD.Common.Model;
using VDCD.DL.Base;

namespace VDCD.DL.Interface
{
    public interface IOrderDL : IBaseDL<Order>
    {
        Task<Order?> GetByIdWithDetailsAsync(Guid id);
        Task<IEnumerable<Order>> GetAllWithDetailsAsync();
    }
}
