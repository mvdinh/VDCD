using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDCD.BL.Interface;
using VDCD.DL.Interface;

namespace VDCD.BL.BaseBL
{
    public class BaseBL<T> : IBaseBL<T> where T : class
    {
        protected readonly IBaseDL<T> _baseDL;

        public BaseBL(IBaseDL<T> baseDL)
        {
            _baseDL = baseDL;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _baseDL.GetAllAsync();
        }

        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            return await _baseDL.GetByIdAsync(id);
        }

        public virtual async Task<int> InsertAsync(T entity)
        {
            await ValidateEntity(entity);
            return await _baseDL.InsertAsync(entity);
        }

        public virtual async Task<int> UpdateAsync(T entity)
        {
            await ValidateEntity(entity);
            return await _baseDL.UpdateAsync(entity);
        }

        public virtual async Task<int> DeleteAsync(Guid id)
        {
            return await _baseDL.DeleteAsync(id);
        }

        protected virtual async Task ValidateEntity(T entity)
        {
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var checkDuplicateAttr = property.GetCustomAttributes(typeof(VDCD.Common.Attribute.Attribute.CheckDuplicateAttribute), true).FirstOrDefault() as VDCD.Common.Attribute.Attribute.CheckDuplicateAttribute;

                if (checkDuplicateAttr != null)
                {
                    var propertyValue = property.GetValue(entity);
                    if (propertyValue != null && !string.IsNullOrWhiteSpace(propertyValue.ToString()))
                    {
                        var propName = property.Name;
                        
                        Guid? excludeId = null;
                        var keyProp = properties.FirstOrDefault(p => p.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.KeyAttribute), true).Any());
                        if (keyProp != null)
                        {
                            var keyValue = keyProp.GetValue(entity);
                            if (keyValue != null && !keyValue.Equals(Guid.Empty))
                            {
                                excludeId = (Guid)keyValue;
                            }
                        }

                        var isDuplicate = await _baseDL.CheckDuplicateAsync(propName, propertyValue, excludeId);
                        if (isDuplicate)
                        {
                            throw new ArgumentException(checkDuplicateAttr.ErrorMessage ?? $"Thông tin {propName} đã tồn tại trong hệ thống.");
                        }
                    }
                }
            }
        }
    }
}
