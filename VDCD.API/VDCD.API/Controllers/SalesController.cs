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
        /// Táº¡o Ä‘Æ¡n hÃ ng má»›i (BÃ¡n hÃ ng cho khÃ¡ch)
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
        /// Xem chi tiáº¿t má»™t Ä‘Æ¡n hÃ ng kÃ¨m danh sÃ¡ch sáº£n pháº©m Ä‘Ã£ mua
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponse>> GetOrderById(Guid id)
        {
            try
            {
                var order = await _salesService.GetOrderByIdAsync(id);
                if (order == null)
                {
                    return NotFound(new { message = $"KhÃ´ng tÃ¬m tháº¥y Ä‘Æ¡n hÃ ng cÃ³ mÃ£ {id}" });
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
        /// Láº¥y danh sÃ¡ch táº¥t cáº£ cÃ¡c hÃ³a Ä‘Æ¡n bÃ¡n hÃ ng
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
