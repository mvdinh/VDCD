using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDCD.Common.DTOs;

namespace VDCD.BL.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponse>> GetAllProductsAsync();
        Task<ProductResponse?> GetProductByIdAsync(Guid id);
        Task<ProductResponse> CreateProductAsync(ProductCreateRequest request);
        Task<ProductResponse?> UpdateProductAsync(Guid id, ProductUpdateRequest request);
        Task<bool> DeleteProductAsync(Guid id);
        Task<IEnumerable<ProductResponse>> SearchProductsAsync(string name);
    }
}
