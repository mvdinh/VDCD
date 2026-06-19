using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDCD.DL.ConnectDB;
using VDCD.DL.Interface;

namespace VDCD.DL.Base
{
    public class BaseDL<T> : IBaseDL<T> where T : class
    {
        protected readonly DBContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseDL(DBContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        /// <summary>
        /// Lấy toàn bộ danh sách bản ghi của một bảng
        /// </summary>
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        /// <summary>
        /// Lấy thông tin chi tiết của một bản ghi dựa vào Khóa chính (ID).
        /// Trả về null nếu không tìm thấy.
        /// </summary>
        /// <param name="id">Khóa chính (GUID)</param>
        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Thêm mới một bản ghi vào Database.
        /// </summary>
        /// <param name="entity">Thực thể cần thêm</param>
        /// <returns>Số lượng bản ghi bị ảnh hưởng (thường là 1 nếu thành công)</returns>
        public virtual async Task<int> InsertAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Cập nhật thông tin của một bản ghi đã tồn tại trong Database.
        /// </summary>
        /// <param name="entity">Thực thể chứa dữ liệu mới</param>
        /// <returns>Số lượng bản ghi bị ảnh hưởng</returns>
        public virtual async Task<int> UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Xóa một bản ghi khỏi Database theo ID.
        /// Đầu tiên tìm kiếm bản ghi, nếu có mới thực hiện xóa.
        /// </summary>
        /// <param name="id">Khóa chính của bản ghi cần xóa</param>
        /// <returns>Số bản ghi bị xóa (1 nếu thành công, 0 nếu không tìm thấy)</returns>
        public virtual async Task<int> DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return 0;

            _dbSet.Remove(entity);
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Hàm kiểm tra xem có bất kỳ bản ghi nào thỏa mãn điều kiện (predicate) hay không.
        /// Rất hữu ích để check các logic nghiệp vụ phức tạp (vd: check ràng buộc khóa ngoại trước khi xóa).
        /// Trả về kết quả ngay khi thấy bản ghi đầu tiên (không đếm toàn bộ).
        /// </summary>
        /// <param name="predicate">Biểu thức điều kiện (Lambda Expression)</param>
        public virtual async Task<bool> AnyAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        /// <summary>
        /// Hàm chuyên dụng để hỗ trợ Validation Attribute [CheckDuplicate].
        /// Tự động build ra câu Query SQL dạng: SELECT EXISTS (SELECT 1 FROM Table WHERE PropertyName = Value AND Id != excludeId).
        /// </summary>
        /// <param name="propertyName">Tên thuộc tính cần check (vd: "CustomerCode", "ProductName")</param>
        /// <param name="value">Giá trị đang nhập từ người dùng</param>
        /// <param name="excludeId">ID của bản ghi hiện tại (chỉ truyền khi đang thực hiện Update để không bị tự báo trùng với chính nó)</param>
        /// <returns>True nếu đã tồn tại dữ liệu trùng, ngược lại là False</returns>
        public virtual async Task<bool> CheckDuplicateAsync(string propertyName, object value, Guid? excludeId = null)
        {
            var parameter = System.Linq.Expressions.Expression.Parameter(typeof(T), "e");
            var left = System.Linq.Expressions.Expression.Property(parameter, propertyName);
            var right = System.Linq.Expressions.Expression.Constant(value);
            System.Linq.Expressions.Expression body = System.Linq.Expressions.Expression.Equal(left, right);

            if (excludeId.HasValue && excludeId.Value != Guid.Empty)
            {
                var keyProps = typeof(T).GetProperties().Where(p => p.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.KeyAttribute), true).Any());
                var keyProp = keyProps.FirstOrDefault();
                if (keyProp != null)
                {
                    var keyLeft = System.Linq.Expressions.Expression.Property(parameter, keyProp.Name);
                    var keyRight = System.Linq.Expressions.Expression.Constant(excludeId.Value);
                    var keyNotEqual = System.Linq.Expressions.Expression.NotEqual(keyLeft, keyRight);
                    body = System.Linq.Expressions.Expression.AndAlso(body, keyNotEqual);
                }
            }

            var predicate = System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(body, parameter);
            return await _dbSet.AnyAsync(predicate);
        }
    }
}
