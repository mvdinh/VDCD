using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VDCD.Common.DTOs;
using VDCD.Common.Model;
using VDCD.DL.Base;
using VDCD.DL.Interface;

namespace VDCD.BL.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductDL _productDL;
        private readonly IBaseDL<Category> _categoryDL;
        private readonly IBaseDL<OrderDetail> _orderDetailDL;

        public ProductService(
            IProductDL productDL,
            IBaseDL<Category> categoryDL,
            IBaseDL<OrderDetail> orderDetailDL)
        {
            _productDL = productDL;
            _categoryDL = categoryDL;
            _orderDetailDL = orderDetailDL;
        }

        public async Task<IEnumerable<ProductResponse>> GetAllProductsAsync()
        {
            var products = await _productDL.GetAllWithCategoryAsync();
            return products.Select(MapToResponse);
        }

        public async Task<ProductResponse?> GetProductByIdAsync(Guid id)
        {
            var product = await _productDL.GetByIdWithCategoryAsync(id);
            if (product == null) return null;
            return MapToResponse(product);
        }

        public async Task<ProductResponse> CreateProductAsync(ProductCreateRequest request)
        {
            // 1. Verify Category exists
            var category = await _categoryDL.GetByIdAsync(request.CategoryId);
            if (category == null)
            {
                throw new ArgumentException($"Danh mục sản phẩm với mã {request.CategoryId} không tồn tại.");
            }

            // 2. Verify SKU is unique (if provided)
            if (!string.IsNullOrWhiteSpace(request.SKU))
            {
                var skuExists = await _productDL.CheckSkuExistsAsync(request.SKU);
                if (skuExists)
                {
                    throw new ArgumentException($"Mã SKU '{request.SKU}' đã tồn tại trong hệ thống.");
                }
            }

            var product = new Common.Model.Product
            {
                ProductId = Guid.NewGuid(),
                CategoryId = request.CategoryId,
                ProductName = request.ProductName,
                SKU = request.SKU,
                Price = request.Price,
                Quantity = request.Quantity,
                Unit = request.Unit,
                Description = request.Description,
                CreatedDate = DateTime.UtcNow
            };

            await _productDL.InsertAsync(product);

            // Fetch again with Category info populated
            var savedProduct = await _productDL.GetByIdWithCategoryAsync(product.ProductId);
            return MapToResponse(savedProduct ?? product);
        }

        public async Task<ProductResponse?> UpdateProductAsync(Guid id, ProductUpdateRequest request)
        {
            var product = await _productDL.GetByIdAsync(id);
            if (product == null) return null;

            // 1. Verify Category exists
            var category = await _categoryDL.GetByIdAsync(request.CategoryId);
            if (category == null)
            {
                throw new ArgumentException($"Danh mục sản phẩm với mã {request.CategoryId} không tồn tại.");
            }

            // 2. Verify SKU is unique (if provided)
            if (!string.IsNullOrWhiteSpace(request.SKU))
            {
                var skuExists = await _productDL.CheckSkuExistsAsync(request.SKU, id);
                if (skuExists)
                {
                    throw new ArgumentException($"Mã SKU '{request.SKU}' đã tồn tại trong hệ thống.");
                }
            }

            product.CategoryId = request.CategoryId;
            product.ProductName = request.ProductName;
            product.SKU = request.SKU;
            product.Price = request.Price;
            product.Quantity = request.Quantity;
            product.Unit = request.Unit;
            product.Description = request.Description;
            product.ModifiedDate = DateTime.UtcNow;

            await _productDL.UpdateAsync(product);

            var savedProduct = await _productDL.GetByIdWithCategoryAsync(id);
            return MapToResponse(savedProduct ?? product);
        }

        public async Task<bool> DeleteProductAsync(Guid id)
        {
            var product = await _productDL.GetByIdAsync(id);
            if (product == null) return false;

            // Check if product is in any orders
            // To do this, we can search in order_details using EF core
            var isInOrders = await _orderDetailDL.AnyAsync(od => od.ProductId == id);
            if (isInOrders)
            {
                throw new InvalidOperationException("Không thể xóa sản phẩm này vì đã phát sinh trong giao dịch bán hàng.");
            }

            await _productDL.DeleteAsync(id);
            return true;
        }

        public async Task<IEnumerable<ProductResponse>> SearchProductsAsync(string name)
        {
            var products = await _productDL.SearchByNameAsync(name);
            return products.Select(MapToResponse);
        }

        private ProductResponse MapToResponse(Common.Model.Product product)
        {
            return new ProductResponse
            {
                ProductId = product.ProductId,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.CategoryName ?? "Chưa phân loại",
                ProductName = product.ProductName,
                SKU = product.SKU,
                Price = product.Price,
                Quantity = product.Quantity,
                Unit = product.Unit,
                Description = product.Description
            };
        }
    }
}
