using System;

namespace VDCD.Common.DTOs
{
    public class CategorySalesQuantityResponse
    {
        public string CategoryName { get; set; } = string.Empty;
        public int TotalQuantitySold { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }

    public class ProductRevenueResponse
    {
        public string ProductName { get; set; } = string.Empty;
        public int TotalQuantitySold { get; set; }
        public decimal TotalRevenue { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
