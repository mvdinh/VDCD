using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VDCD.BL.Interface
{
    public interface IBaseBL<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task<int> InsertAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task<int> DeleteAsync(Guid id);
    }
}
