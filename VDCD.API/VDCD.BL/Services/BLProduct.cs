using VDCD.BL.Interface;
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
    public class BLProduct : VDCD.BL.BaseBL.BaseBL<Product>, IBLProduct
    {
        private readonly IProductDL _productDL;
        private readonly IBaseDL<Category> _categoryDL;
        private readonly IBaseDL<Unit> _unitDL;
        private readonly IBaseDL<OrderDetail> _orderDetailDL;
        private readonly IBaseDL<User> _userDL;

        public BLProduct(
            IProductDL productDL,
            IBaseDL<Category> categoryDL,
            IBaseDL<Unit> unitDL,
            IBaseDL<OrderDetail> orderDetailDL,
            IBaseDL<User> userDL) : base(productDL)
        {
            _productDL = productDL;
            _categoryDL = categoryDL;
            _unitDL = unitDL;
            _orderDetailDL = orderDetailDL;
            _userDL = userDL;
        }

        public async Task<IEnumerable<ProductResponse>> GetAllProductsAsync()
        {
            var products = await _productDL.GetAllWithCategoryAsync();
            var users = await _userDL.GetAllAsync();
            var userDict = users.ToDictionary(u => u.UserId, u => u.UserName);
            return products.Select(p => MapToResponse(p, userDict));
        }

        public async Task<ProductResponse?> GetProductByIdAsync(Guid id)
        {
            var product = await _productDL.GetByIdWithCategoryAsync(id);
            if (product == null) return null;
            var users = await _userDL.GetAllAsync();
            var userDict = users.ToDictionary(u => u.UserId, u => u.UserName);
            return MapToResponse(product, userDict);
        }

        public async Task<ProductResponse> InsertProductAsync(ProductCreateRequest request)
        {
            // 0. Verify User
            var user = await _userDL.GetByIdAsync(request.CreatedBy);
            if (user == null)
            {
                throw new ArgumentException($"Người dùng với mã {request.CreatedBy} không tồn tại.");
            }

            // 1. Verify Category exists
            var category = await _categoryDL.GetByIdAsync(request.CategoryId);
            if (category == null)
            {
                throw new ArgumentException($"{request.CategoryId} không tồn tại");
            }

            // 1.5. Verify Unit exists
            var unit = await _unitDL.GetByIdAsync(request.UnitId);
            if (unit == null)
            {
                throw new ArgumentException($"{request.UnitId} không tồn tại");
            }



            var product = new Common.Model.Product
            {
                ProductId = Guid.NewGuid(),
                CategoryId = request.CategoryId,
                ProductName = request.ProductName,
                Price = request.Price,
                Quantity = request.Quantity,
                UnitId = request.UnitId,
                Description = request.Description,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = request.CreatedBy
            };

            await base.InsertAsync(product);

            // Fetch again with Category info populated
            var savedProduct = await _productDL.GetByIdWithCategoryAsync(product.ProductId);
            var users = await _userDL.GetAllAsync();
            var userDict = users.ToDictionary(u => u.UserId, u => u.UserName);
            return MapToResponse(savedProduct ?? product, userDict);
        }

        public async Task<ProductResponse?> UpdateProductAsync(Guid id, ProductUpdateRequest request)
        {
            var product = await _productDL.GetByIdAsync(id);
            if (product == null) return null;

            // 0. Verify User
            var user = await _userDL.GetByIdAsync(request.ModifiedBy);
            if (user == null)
            {
                throw new ArgumentException($"Người dùng với mã {request.ModifiedBy} không tồn tại.");
            }

            // 1. Verify Category exists
            var category = await _categoryDL.GetByIdAsync(request.CategoryId);
            if (category == null)
            {
                throw new ArgumentException($"{request.CategoryId} không tồn tại");
            }

            // 1.5. Verify Unit exists
            var unit = await _unitDL.GetByIdAsync(request.UnitId);
            if (unit == null)
            {
                throw new ArgumentException($"{request.UnitId} không tồn tại");
            }



            product.CategoryId = request.CategoryId;
            product.ProductName = request.ProductName;
            product.Price = request.Price;
            product.Quantity = request.Quantity;
            product.UnitId = request.UnitId;
            product.Description = request.Description;
            product.ModifiedDate = DateTime.UtcNow;
            product.ModifiedBy = request.ModifiedBy;

            await base.UpdateAsync(product);

            var savedProduct = await _productDL.GetByIdWithCategoryAsync(id);
            var users = await _userDL.GetAllAsync();
            var userDict = users.ToDictionary(u => u.UserId, u => u.UserName);
            return MapToResponse(savedProduct ?? product, userDict);
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
                throw new InvalidOperationException("KhÃ´ng thá»ƒ xÃ³a sáº£n pháº©m nÃ y vÃ¬ Ä‘Ã£ phÃ¡t sinh trong giao dá»‹ch bÃ¡n hÃ ng.");
            }

            await _productDL.DeleteAsync(id);
            return true;
        }

        public async Task<IEnumerable<ProductResponse>> SearchProductsAsync(string name)
        {
            var products = await _productDL.SearchByNameAsync(name);
            var users = await _userDL.GetAllAsync();
            var userDict = users.ToDictionary(u => u.UserId, u => u.UserName);
            return products.Select(p => MapToResponse(p, userDict));
        }

        private ProductResponse MapToResponse(Common.Model.Product product, Dictionary<Guid, string> userDict)
        {
            return new ProductResponse
            {
                ProductId = product.ProductId,
                CategoryName = product.Category?.CategoryName ,
                ProductName = product.ProductName,
                Price = product.Price,
                Quantity = product.Quantity,
                UnitName = product.Unit?.UnitName ?? "",
                Description = product.Description,
                CreatedBy = product.CreatedBy.HasValue && userDict.ContainsKey(product.CreatedBy.Value) ? userDict[product.CreatedBy.Value] : "Unknown User",
                CreatedDate = product.CreatedDate,
                ModifiedBy = product.ModifiedBy.HasValue && userDict.ContainsKey(product.ModifiedBy.Value) ? userDict[product.ModifiedBy.Value] : "Unknown User",
                ModifiedDate = product.ModifiedDate
            };
        }
    }
}
