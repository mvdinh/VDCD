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
    public class ProductsController : ControllerBase
    {
        private readonly IBLProduct _productService;

        public ProductsController(IBLProduct productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Lấy tất cả sản phẩm
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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
                return StatusCode(500, new ErrorResult 
                   {
                    DevMsg = ex.Message,
                    UserMsg = Resource1.Exception,
                    MoreInfo = ex.Data,
                });
            }
        }

        /// <summary>
        /// Lấy sản phẩm theo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponse>> GetProductById(Guid id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    return StatusCode(404, new ErrorResult
                    {
                        DevMsg = "Không tìm thấy sản phẩm",
                        UserMsg = Resource1.Exception,
                    });
                }
                return Ok(product);
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
        /// Thêm mới sản phẩm
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
                var created = await _productService.InsertProductAsync(request);
                return CreatedAtAction(nameof(GetProductById), new { id = created.ProductId }, created);
            }
            catch (ArgumentException ex)
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
        /// Cập nhật thông tin sản phẩm
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
                    return StatusCode(404, new ErrorResult
                    {
                        DevMsg = "Không tìm thấy sản phẩm",
                        UserMsg = Resource1.Exception,
                    });
                }
                return Ok(updated);
            }
            catch (ArgumentException ex)
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
        /// Xóa sản phẩm
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            try
            {
                var success = await _productService.DeleteProductAsync(id);
                if (!success)
                {
                    return StatusCode(404, new ErrorResult
                    {
                        DevMsg = "Không tìm thấy sản phẩm",
                        UserMsg = Resource1.Exception,
                    });
                }
                return Ok(new { message = "Xóa sản phẩm thành công." });
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
    }
}
