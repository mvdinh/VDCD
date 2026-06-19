using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VDCD.Common.DTOs
{
    public class CreateOrderRequest
    {
        public Guid? CustomerId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giảm giá phải lớn hơn hoặc bằng 0.")]
        public decimal Discount { get; set; }

        [Required(ErrorMessage = "Phương thức thanh toán không được để trống.")]
        [StringLength(50, ErrorMessage = "Phương thức thanh toán không quá 50 ký tự.")]
        public string PaymentMethod { get; set; } = "Tiền mặt";

        public string? Note { get; set; }

        [Required(ErrorMessage = "Đơn hàng phải có ít nhất một sản phẩm.")]
        [MinLength(1, ErrorMessage = "Đơn hàng phải có ít nhất một sản phẩm.")]
        public List<OrderDetailRequest> OrderDetails { get; set; } = new List<OrderDetailRequest>();
    }

    public class OrderDetailRequest
    {
        [Required(ErrorMessage = "Mã sản phẩm không được để trống.")]
        public Guid ProductId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Số lượng mua phải lớn hơn hoặc bằng 1.")]
        public int Quantity { get; set; }

        public decimal? UnitPrice { get; set; } // Client can supply or use DB product price
    }

    public class OrderResponse
    {
        public Guid OrderId { get; set; }
        public Guid? CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalAmount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string? Note { get; set; }
        public List<OrderDetailResponse> OrderDetails { get; set; } = new List<OrderDetailResponse>();
    }

    public class OrderDetailResponse
    {
        public Guid OrderDetailId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
    }
}
