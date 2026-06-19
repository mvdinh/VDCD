using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VDCD.Common.DTOs;
using VDCD.DL;
using VDCD.DL.ConnectDB;

namespace VDCD.BL.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly DBContext _context;

        public StatisticsService(DBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategorySalesQuantityResponse>> GetCategorySalesQuantityAsync(DateTime? fromDate, DateTime? toDate)
        {
            var start = fromDate ?? DateTime.MinValue;
            var end = toDate ?? DateTime.MaxValue;

            // Group order details by Category Name and calculate total quantity sold
            var report = await _context.OrderDetails
                .Include(od => od.Order)
                .Include(od => od.Product)
                    .ThenInclude(p => p!.Category)
                .Where(od => od.Order!.OrderDate >= start && od.Order!.OrderDate <= end)
                .GroupBy(od => od.Product!.Category!.CategoryName)
                .Select(g => new CategorySalesQuantityResponse
                {
                    CategoryName = g.Key ?? "Chưa phân loại",
                    TotalQuantitySold = g.Sum(od => od.Quantity),
                    FromDate = start,
                    ToDate = end
                })
                .OrderByDescending(r => r.TotalQuantitySold)
                .ToListAsync();

            return report;
        }

        public async Task<ProductRevenueResponse?> GetProductRevenueAsync(Guid productId, DateTime? fromDate, DateTime? toDate)
        {
            var start = fromDate ?? DateTime.MinValue;
            var end = toDate ?? DateTime.MaxValue;

            var product = await _context.Products.FindAsync(productId);
            if (product == null) return null;

            var orderDetails = await _context.OrderDetails
                .Include(od => od.Order)
                .Where(od => od.ProductId == productId && od.Order!.OrderDate >= start && od.Order!.OrderDate <= end)
                .ToListAsync();

            int totalQuantitySold = orderDetails.Sum(od => od.Quantity);
            decimal totalRevenue = orderDetails.Sum(od => od.SubTotal);

            return new ProductRevenueResponse
            {
                ProductName = product.ProductName,
                TotalQuantitySold = totalQuantitySold,
                TotalRevenue = totalRevenue,
                FromDate = start,
                ToDate = end
            };
        }
    }
}
