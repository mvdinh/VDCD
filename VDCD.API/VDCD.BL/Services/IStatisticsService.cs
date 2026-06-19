using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDCD.Common.DTOs;

namespace VDCD.BL.Services
{
    public interface IStatisticsService
    {
        Task<IEnumerable<CategorySalesQuantityResponse>> GetCategorySalesQuantityAsync(DateTime? fromDate, DateTime? toDate);
        Task<ProductRevenueResponse?> GetProductRevenueAsync(Guid productId, DateTime? fromDate, DateTime? toDate);
    }
}
