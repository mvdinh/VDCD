using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDCD.Common.Model;
using VDCD.DL.Base;
using VDCD.DL.Interface;

namespace VDCD.DL.Interface
{
    public interface IProductDL : IBaseDL<Product>
    {
        Task<IEnumerable<Product>> GetAllWithCategoryAsync();
        Task<Product?> GetByIdWithCategoryAsync(Guid id);
        Task<IEnumerable<Product>> SearchByNameAsync(string name);
        Task<bool> CheckSkuExistsAsync(string sku, Guid? excludeProductId = null);
    }
}
