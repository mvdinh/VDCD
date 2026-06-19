using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VDCD.DL.Base;
using VDCD.DL.Interface;
using VDCD.DL.ConnectDB;
using VDCD.Common.Model;

namespace VDCD.DL.Repo
{
    public class ProductDL : BaseDL<Product>, IProductDL
    {
        public ProductDL(DBContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetAllWithCategoryAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<Product?> GetByIdWithCategoryAsync(Guid id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<IEnumerable<Product>> SearchByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return await GetAllWithCategoryAsync();

            return await _context.Products
                .Include(p => p.Category)
                .Where(p => EF.Functions.ILike(p.ProductName, $"%{name}%"))
                .ToListAsync();
        }

        public async Task<bool> CheckSkuExistsAsync(string sku, Guid? excludeProductId = null)
        {
            if (excludeProductId.HasValue)
            {
                return await _context.Products.AnyAsync(p => p.SKU == sku && p.ProductId != excludeProductId.Value);
            }
            return await _context.Products.AnyAsync(p => p.SKU == sku);
        }
    }
}
