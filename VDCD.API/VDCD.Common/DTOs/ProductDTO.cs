using System;
using System.ComponentModel.DataAnnotations;
using VDCDRequired = VDCD.Common.Attribute.Attribute.RequiredAttribute;

namespace VDCD.Common.DTOs
{
    public class ProductCreateRequest
    {
        [VDCDRequired]
        public Guid CategoryId { get; set; }

        [VDCDRequired]
        [StringLength(200, ErrorMessage = "Tên sản phẩm không được dài quá 200 ký tự.")]
        public string ProductName { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Mã SKU không được dài quá 50 ký tự.")]
        public string? SKU { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn hoặc bằng 0.")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số lượng sản phẩm phải lớn hơn hoặc bằng 0.")]
        public int Quantity { get; set; }

        [VDCDRequired]
        [StringLength(30, ErrorMessage = "Đơn vị tính không được dài quá 30 ký tự.")]
        public string Unit { get; set; } = "Cái";

        public string? Description { get; set; }
    }

    public class ProductUpdateRequest
    {
        [VDCDRequired]
        public Guid CategoryId { get; set; }

        [VDCDRequired]
        [StringLength(200, ErrorMessage = "Tên sản phẩm không được dài quá 200 ký tự.")]
        public string ProductName { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Mã SKU không được dài quá 50 ký tự.")]
        public string? SKU { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn hoặc bằng 0.")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số lượng sản phẩm phải lớn hơn hoặc bằng 0.")]
        public int Quantity { get; set; }

        [VDCDRequired]
        [StringLength(30, ErrorMessage = "Đơn vị tính không được dài quá 30 ký tự.")]
        public string Unit { get; set; } = "Cái";

        public string? Description { get; set; }
    }

    public class ProductResponse
    {
        public Guid ProductId { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string? SKU { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; } = "Cái";
        public string? Description { get; set; }
    }
}
