using VDCD.BL.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDCD.BL.Services;
using VDCD.Common.DTOs;
using VDCD.Common.Model;
using VDCD.Common.Resources;

namespace VDCD.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly IBLStatistics _statisticsService;

        public StatisticsController(IBLStatistics statisticsService)
        {
            _statisticsService = statisticsService;
        }

        /// <summary>
        /// Thá»‘ng kÃª sá»‘ lÆ°á»£ng bÃ¡n Ä‘Æ°á»£c trong ngÃ y hoáº·c khoáº£ng thá»i gian cá»§a má»—i loáº¡i sáº£n pháº©m (Category)
        /// </summary>
        [HttpGet("category-sales")]
        public async Task<ActionResult<IEnumerable<CategorySalesQuantityResponse>>> GetCategorySalesQuantity(
            [FromQuery] DateTime? date,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate)
        {
            try
            {
                if (date.HasValue)
                {
                    fromDate = date.Value.Date;
                    toDate = date.Value.Date.AddDays(1).AddTicks(-1);
                }

                var report = await _statisticsService.GetCategorySalesQuantityAsync(fromDate, toDate);
                return Ok(report);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResult
                {
                    DevMsg = ex.Message,
                    UserMsg = Resource1.Exception,
                    MoreInfo = ex.Data,
                });
            }
        }

        /// <summary>
        /// Tra cá»©u sá»‘ tiá»n bÃ¡n Ä‘Æ°á»£c (doanh thu) cá»§a 1 sáº£n pháº©m (Product) trong khoáº£ng thá»i gian
        /// </summary>
        [HttpGet("product-revenue/{productId}")]
        public async Task<ActionResult<ProductRevenueResponse>> GetProductRevenue(
            Guid productId,
            [FromQuery] DateTime? date,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate)
        {
            try
            {
                if (date.HasValue)
                {
                    fromDate = date.Value.Date;
                    toDate = date.Value.Date.AddDays(1).AddTicks(-1);
                }

                var report = await _statisticsService.GetProductRevenueAsync(productId, fromDate, toDate);
                if (report == null)
                {
                    return NotFound(new { message = $"Không tìm thấy sản phẩm có mã {productId}" });
                }
                return Ok(report);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResult
                {
                    DevMsg = ex.Message,
                    UserMsg = Resource1.Exception,
                    MoreInfo = ex.Data,
                });
            }
        }
    }
}
