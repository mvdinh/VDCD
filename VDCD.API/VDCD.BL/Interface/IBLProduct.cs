using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDCD.Common.DTOs;
using VDCD.Common.Model;

namespace VDCD.BL.Interface
{
    public interface IBLProduct : IBaseBL<Product>
    {
        Task<IEnumerable<ProductResponse>> GetAllProductsAsync();
        Task<ProductResponse?> GetProductByIdAsync(Guid id);
        Task<ProductResponse> InsertProductAsync(ProductCreateRequest request);
        Task<ProductResponse?> UpdateProductAsync(Guid id, ProductUpdateRequest request);
        Task<bool> DeleteProductAsync(Guid id);
        Task<IEnumerable<ProductResponse>> SearchProductsAsync(string name);
    }
}
