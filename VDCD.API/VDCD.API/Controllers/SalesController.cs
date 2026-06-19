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
    public class SalesController : ControllerBase
    {
        private readonly ISalesService _salesService;

        public SalesController(ISalesService salesService)
        {
            _salesService = salesService;
        }

        /// <summary>
        /// Tạo đơn hàng mới (Bán hàng cho khách)
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<OrderResponse>> CreateOrder([FromBody] CreateOrderRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _salesService.CreateOrderAsync(request);
                return CreatedAtAction(nameof(GetOrderById), new { id = response.OrderId }, response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống khi tạo đơn hàng.", details = ex.Message });
            }
        }

        /// <summary>
        /// Xem chi tiết một đơn hàng kèm danh sách sản phẩm đã mua
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponse>> GetOrderById(Guid id)
        {
            try
            {
                var order = await _salesService.GetOrderByIdAsync(id);
                if (order == null)
                {
                    return NotFound(new { message = $"Không tìm thấy đơn hàng có mã {id}" });
                }
                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống.", details = ex.Message });
            }
        }

        /// <summary>
        /// Lấy danh sách tất cả các hóa đơn bán hàng
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetAllOrders()
        {
            try
            {
                var orders = await _salesService.GetAllOrdersAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống.", details = ex.Message });
            }
        }
    }
}
