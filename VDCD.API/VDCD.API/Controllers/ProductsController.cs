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
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Xem danh sách mặt hàng gốm sứ (Hỗ trợ tìm kiếm theo tên)
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProducts([FromQuery] string? name)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(name))
                {
                    var searched = await _productService.SearchProductsAsync(name);
                    return Ok(searched);
                }

                var products = await _productService.GetAllProductsAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = VDCD.Common.Resources.Resource1.Exception, details = ex.Message });
            }
        }

        /// <summary>
        /// Xem chi tiết một mặt hàng gốm sứ
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponse>> GetProductById(Guid id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound(new { message = $"Không tìm thấy sản phẩm có mã {id}" });
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = VDCD.Common.Resources.Resource1.Exception, details = ex.Message });
            }
        }

        /// <summary>
        /// Thêm mới mặt hàng gốm sứ
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ProductResponse>> CreateProduct([FromBody] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var created = await _productService.CreateProductAsync(request);
                return CreatedAtAction(nameof(GetProductById), new { id = created.ProductId }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = VDCD.Common.Resources.Resource1.Exception, details = ex.Message });
            }
        }

        /// <summary>
        /// Cập nhật thông tin mặt hàng gốm sứ
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductResponse>> UpdateProduct(Guid id, [FromBody] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updated = await _productService.UpdateProductAsync(id, request);
                if (updated == null)
                {
                    return NotFound(new { message = $"Không tìm thấy sản phẩm có mã {id} để cập nhật" });
                }
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = VDCD.Common.Resources.Resource1.Exception, details = ex.Message });
            }
        }

        /// <summary>
        /// Xóa mặt hàng gốm sứ
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            try
            {
                var success = await _productService.DeleteProductAsync(id);
                if (!success)
                {
                    return NotFound(new { message = $"Không tìm thấy sản phẩm có mã {id} để xóa" });
                }
                return Ok(new { message = "Xóa sản phẩm thành công." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = VDCD.Common.Resources.Resource1.Exception, details = ex.Message });
            }
        }
    }
}
