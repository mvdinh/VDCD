using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDCD.DL.Base;
using VDCD.DL.Interface;
using VDCD.DL.ConnectDB;
using VDCD.Common.Model;

namespace VDCD.DL.Repo
{
    public class OrderDL : BaseDL<Order>, IOrderDL
    {
        public OrderDL(DBContext context) : base(context)
        {
        }

        public async Task<Order?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.OrderId == id);
        }

        public async Task<IEnumerable<Order>> GetAllWithDetailsAsync()
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .ToListAsync();
        }
    }
}
