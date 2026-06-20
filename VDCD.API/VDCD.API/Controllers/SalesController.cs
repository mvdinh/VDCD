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
    public class SalesController : ControllerBase
    {
        private readonly IBLOrder _salesService;

        public SalesController(IBLOrder salesService)
        {
            _salesService = salesService;
        }

        /// <summary>
        /// tạo đơn hàng mới
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
                var response = await _salesService.InsertOrderAsync(request);
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
                return StatusCode(500, new ErrorResult
                {
                    DevMsg = ex.Message,
                    UserMsg = Resource1.Exception,
                    MoreInfo = ex.Data,
                });
            }
        }

        /// <summary>
        /// Xem chi tiết đơn hàng
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
                return StatusCode(500, new ErrorResult
                {
                    DevMsg = ex.Message,
                    UserMsg = Resource1.Exception,
                    MoreInfo = ex.Data,
                });
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
