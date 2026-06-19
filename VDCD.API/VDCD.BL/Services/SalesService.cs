using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VDCD.Common.DTOs;
using VDCD.Common.Model;
using VDCD.DL.Base;
using VDCD.DL.Interface;

namespace VDCD.BL.Services
{
    public class SalesService : ISalesService
    {
        private readonly IOrderDL _orderDL;
        private readonly IProductDL _productDL;
        private readonly IBaseDL<Customer> _customerDL;

        public SalesService(
            IOrderDL orderDL,
            IProductDL productDL,
            IBaseDL<Customer> customerDL)
        {
            _orderDL = orderDL;
            _productDL = productDL;
            _customerDL = customerDL;
        }

        public async Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request)
        {
            // 1. Verify Customer exists if provided
            if (request.CustomerId.HasValue)
            {
                var customer = await _customerDL.GetByIdAsync(request.CustomerId.Value);
                if (customer == null)
                {
                    throw new ArgumentException($"Khách hàng với mã {request.CustomerId.Value} không tồn tại.");
                }
            }

            // 2. Validate Order Details, stock, and calculate amounts
            var orderDetails = new List<OrderDetail>();
            decimal totalAmount = 0;

            foreach (var item in request.OrderDetails)
            {
                var product = await _productDL.GetByIdAsync(item.ProductId);
                if (product == null)
                {
                    throw new ArgumentException($"Sản phẩm với mã {item.ProductId} không tồn tại.");
                }

                if (product.Quantity < item.Quantity)
                {
                    throw new InvalidOperationException($"Sản phẩm '{product.ProductName}' không đủ tồn kho. Hiện còn: {product.Quantity}, yêu cầu: {item.Quantity}.");
                }

                // Check unit price (use provided or product default price)
                decimal unitPrice = item.UnitPrice ?? product.Price;
                decimal subTotal = item.Quantity * unitPrice;
                totalAmount += subTotal;

                // Deduct stock in C# (so it works without database triggers)
                product.Quantity -= item.Quantity;
                await _productDL.UpdateAsync(product);

                var detail = new OrderDetail
                {
                    OrderDetailId = Guid.NewGuid(),
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = unitPrice,
                    SubTotal = subTotal, // Handled in C# for consistency
                    CreatedDate = DateTime.UtcNow
                };

                orderDetails.Add(detail);
            }

            // 3. Create Order
            var orderId = Guid.NewGuid();
            var order = new Order
            {
                OrderId = orderId,
                CustomerId = request.CustomerId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                Discount = request.Discount,
                FinalAmount = Math.Max(0, totalAmount - request.Discount), // Handled in C#
                PaymentMethod = request.PaymentMethod,
                Note = request.Note,
                CreatedDate = DateTime.UtcNow
            };

            foreach (var detail in orderDetails)
            {
                detail.OrderId = orderId;
                order.OrderDetails.Add(detail);
            }

            await _orderDL.InsertAsync(order);

            // Fetch the fully populated order to return
            var savedOrder = await _orderDL.GetByIdWithDetailsAsync(orderId);
            if (savedOrder == null)
            {
                throw new InvalidOperationException("Lỗi hệ thống khi tải thông tin đơn hàng sau khi tạo.");
            }

            return MapToResponse(savedOrder);
        }

        public async Task<OrderResponse?> GetOrderByIdAsync(Guid id)
        {
            var order = await _orderDL.GetByIdWithDetailsAsync(id);
            if (order == null) return null;
            return MapToResponse(order);
        }

        public async Task<IEnumerable<OrderResponse>> GetAllOrdersAsync()
        {
            var orders = await _orderDL.GetAllWithDetailsAsync();
            return orders.Select(MapToResponse);
        }

        private OrderResponse MapToResponse(Order order)
        {
            return new OrderResponse
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                CustomerName = order.Customer?.CustomerName ?? "Khách vãng lai",
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Discount = order.Discount,
                FinalAmount = order.FinalAmount,
                PaymentMethod = order.PaymentMethod,
                Note = order.Note,
                OrderDetails = order.OrderDetails.Select(od => new OrderDetailResponse
                {
                    OrderDetailId = od.OrderDetailId,
                    ProductId = od.ProductId,
                    ProductName = od.Product?.ProductName ?? "Sản phẩm không tên",
                    Quantity = od.Quantity,
                    UnitPrice = od.UnitPrice,
                    SubTotal = od.SubTotal
                }).ToList()
            };
        }
    }
}
