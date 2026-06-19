using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDCD.Common.DTOs;

namespace VDCD.BL.Services
{
    public interface ISalesService
    {
        Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request);
        Task<OrderResponse?> GetOrderByIdAsync(Guid id);
        Task<IEnumerable<OrderResponse>> GetAllOrdersAsync();
    }
}
