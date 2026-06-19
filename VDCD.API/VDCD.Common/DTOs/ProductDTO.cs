using System;
using System.ComponentModel.DataAnnotations;
using VDCDRequired = VDCD.Common.Attribute.Attribute.RequiredAttribute;

namespace VDCD.Common.DTOs
{
    public class ProductCreateRequest
    {
        public Guid CategoryId { get; set; }


        [VDCDRequired]
        public string ProductName { get; set; }

        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số lượng sản phẩm phải lớn hơn hoặc bằng 0.")]
        public int Quantity { get; set; }

        [VDCDRequired]
        public Guid UnitId { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Người tạo không được để trống.")]
        public Guid CreatedBy { get; set; }
    }

    public class ProductUpdateRequest
    {
        [VDCDRequired]
        public Guid CategoryId { get; set; }

        [VDCDRequired]
        [StringLength(200, ErrorMessage = "Tên sản phẩm không được dài quá 200 ký tự.")]
        public string ProductName { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn hoặc bằng 0.")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số lượng sản phẩm phải lớn hơn hoặc bằng 0.")]
        public int Quantity { get; set; }

        [VDCDRequired]
        public Guid UnitId { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Người sửa không được để trống.")]
        public Guid ModifiedBy { get; set; }
    }

    public class ProductResponse
    {
        public Guid ProductId { get; set; }
        public string CategoryName { get; set; } 
        public string ProductName { get; set; } 
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string UnitName { get; set; } = string.Empty;
        public string? Description { get; set; }

        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; } = string.Empty;
        public DateTime? ModifiedDate { get; set; }
    }
}
