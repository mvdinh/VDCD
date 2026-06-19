using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDCD.BL.Services;
using VDCD.Common.DTOs;

namespace VDCD.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        /// <summary>
        /// Thống kê số lượng bán được trong ngày hoặc khoảng thời gian của mỗi loại sản phẩm (Category)
        /// </summary>
        [HttpGet("category-sales")]
        public async Task<ActionResult<IEnumerable<CategorySalesQuantityResponse>>> GetCategorySalesQuantity(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate)
        {
            try
            {
                var report = await _statisticsService.GetCategorySalesQuantityAsync(fromDate, toDate);
                return Ok(report);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống khi lấy báo cáo thống kê danh mục.", details = ex.Message });
            }
        }

        /// <summary>
        /// Tra cứu số tiền bán được (doanh thu) của 1 sản phẩm (Product) trong khoảng thời gian
        /// </summary>
        [HttpGet("product-revenue/{productId}")]
        public async Task<ActionResult<ProductRevenueResponse>> GetProductRevenue(
            Guid productId,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate)
        {
            try
            {
                var report = await _statisticsService.GetProductRevenueAsync(productId, fromDate, toDate);
                if (report == null)
                {
                    return NotFound(new { message = $"Không tìm thấy sản phẩm có mã {productId}" });
                }
                return Ok(report);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống khi tra cứu doanh số.", details = ex.Message });
            }
        }
    }
}
