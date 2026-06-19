using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDCD.Common.DTOs;

namespace VDCD.BL.Interface
{
    public interface IBLOrder : VDCD.BL.Interface.IBaseBL<VDCD.Common.Model.Order>
    {
        Task<OrderResponse> InsertOrderAsync(CreateOrderRequest request);
        Task<OrderResponse?> GetOrderByIdAsync(Guid id);
        Task<IEnumerable<OrderResponse>> GetAllOrdersAsync();
    }
}
