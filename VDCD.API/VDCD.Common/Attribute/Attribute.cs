using System;
using System.ComponentModel.DataAnnotations;
using VDCD.Common.Resources;

namespace VDCD.Common.Attribute
{
    public class Attribute
    {
        [AttributeUsage(AttributeTargets.Property)]
        public class RequiredAttribute : System.ComponentModel.DataAnnotations.RequiredAttribute
        {
            public RequiredAttribute()
            {
                ErrorMessage = Resource1.EmptyError ?? "Thông tin này không được để trống";
            }

            public RequiredAttribute(string errorMessage)
            {
                ErrorMessage = !string.IsNullOrEmpty(errorMessage) ? errorMessage : (Resource1.EmptyError ?? "Thông tin này không được để trống");
            }
        }

        [AttributeUsage(AttributeTargets.Property)]
        public class CheckDuplicateAttribute : ValidationAttribute
        {
            public CheckDuplicateAttribute()
            {
                ErrorMessage = "Thông tin đã tồn tại trong hệ thống";
            }

            public CheckDuplicateAttribute(string errorMessage)
            {
                ErrorMessage = !string.IsNullOrEmpty(errorMessage) ? errorMessage : "Thông tin đã tồn tại trong hệ thống";
            }

            protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
            {
                // Logic check duplicate sẽ được xử lý ở Service, hoặc thêm logic vào đây nếu cần thiết
                return ValidationResult.Success;
            }
        }
    }
}
