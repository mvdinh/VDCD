using VDCD.BL.Interface;
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
    public class BLOrder : VDCD.BL.BaseBL.BaseBL<Order>, IBLOrder
    {
        private readonly IOrderDL _orderDL;
        private readonly IProductDL _productDL;
        private readonly IBaseDL<Customer> _customerDL;
        private readonly IBaseDL<User> _userDL;

        public BLOrder(
            IOrderDL orderDL,
            IProductDL productDL,
            IBaseDL<Customer> customerDL,
            IBaseDL<User> userDL) : base(orderDL)
        {
            _orderDL = orderDL;
            _productDL = productDL;
            _customerDL = customerDL;
            _userDL = userDL;
        }

        public async Task<OrderResponse> InsertOrderAsync(CreateOrderRequest request)
        {
            // 0. Verify User
            var user = await _userDL.GetByIdAsync(request.CreatedBy);
            if (user == null)
            {
                throw new ArgumentException($"Người dùng với mã {request.CreatedBy} không tồn tại.");
            }

            // 1. Verify Customer exists if provided
            if (request.CustomerId.HasValue && request.CustomerId.Value != Guid.Empty)
            {
                var customer = await _customerDL.GetByIdAsync(request.CustomerId.Value);
                if (customer == null)
                {
                    throw new ArgumentException($"KhÃ¡ch hÃ ng vá»›i mÃ£ {request.CustomerId.Value} khÃ´ng tá»“n táº¡i.");
                }
            }
            else if (!string.IsNullOrWhiteSpace(request.CustomerName))
            {
                // Auto-create new customer
                var newCustomer = new Customer
                {
                    CustomerId = Guid.NewGuid(),
                    CustomerName = request.CustomerName,
                    PhoneNumber = request.PhoneNumber,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = request.CreatedBy
                };
                await _customerDL.InsertAsync(newCustomer);
                request.CustomerId = newCustomer.CustomerId;
            }

            // 2. Validate Order Details, stock, and calculate amounts
            var orderDetails = new List<OrderDetail>();
            decimal totalAmount = 0;

            foreach (var item in request.OrderDetails)
            {
                var product = await _productDL.GetByIdAsync(item.ProductId);
                if (product == null)
                {
                    throw new ArgumentException($"Sáº£n pháº©m vá»›i mÃ£ {item.ProductId} khÃ´ng tá»“n táº¡i.");
                }

                if (product.Quantity < item.Quantity)
                {
                    throw new InvalidOperationException($"Sáº£n pháº©m '{product.ProductName}' khÃ´ng Ä‘á»§ tá»“n kho. Hiá»‡n cÃ²n: {product.Quantity}, yÃªu cáº§u: {item.Quantity}.");
                }

                // Enforce unit price to always be the DB product price
                decimal unitPrice = product.Price;
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
                    SubTotal = subTotal, // SubTotal = Quantity * DB UnitPrice
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = request.CreatedBy
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
                CreatedDate = DateTime.UtcNow,
                CreatedBy = request.CreatedBy
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
                throw new InvalidOperationException("Lá»—i há»‡ thá»‘ng khi táº£i thÃ´ng tin Ä‘Æ¡n hÃ ng sau khi táº¡o.");
            }

            var users = await _userDL.GetAllAsync();
            var userDict = users.ToDictionary(u => u.UserId, u => u.UserName);

            return MapToResponse(savedOrder, userDict);
        }

        public async Task<OrderResponse?> GetOrderByIdAsync(Guid id)
        {
            var order = await _orderDL.GetByIdWithDetailsAsync(id);
            if (order == null) return null;
            
            var users = await _userDL.GetAllAsync();
            var userDict = users.ToDictionary(u => u.UserId, u => u.UserName);
            
            return MapToResponse(order, userDict);
        }

        public async Task<IEnumerable<OrderResponse>> GetAllOrdersAsync()
        {
            var orders = await _orderDL.GetAllWithDetailsAsync();
            var users = await _userDL.GetAllAsync();
            var userDict = users.ToDictionary(u => u.UserId, u => u.UserName);
            
            return orders.Select(o => MapToResponse(o, userDict));
        }

        private OrderResponse MapToResponse(Order order, Dictionary<Guid, string> userDict)
        {
            return new OrderResponse
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                CustomerName = order.Customer?.CustomerName ?? "KhÃ¡ch vÃ£ng lai",
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Discount = order.Discount,
                FinalAmount = order.FinalAmount,
                PaymentMethod = order.PaymentMethod,
                Note = order.Note,
                CreatedBy = order.CreatedBy.HasValue && userDict.ContainsKey(order.CreatedBy.Value) ? userDict[order.CreatedBy.Value] : "Unknown User",
                CreatedDate = order.CreatedDate,
                ModifiedBy = order.ModifiedBy.HasValue && userDict.ContainsKey(order.ModifiedBy.Value) ? userDict[order.ModifiedBy.Value] : "Unknown User",
                ModifiedDate = order.ModifiedDate,
                OrderDetails = order.OrderDetails.Select(od => new OrderDetailResponse
                {
                    ProductId = od.ProductId,
                    ProductName = od.Product?.ProductName ?? "Sáº£n pháº©m khÃ´ng tÃªn",
                    Quantity = od.Quantity,
                    UnitPrice = od.UnitPrice,
                    SubTotal = od.SubTotal
                }).ToList()
            };
        }
    }
}
