using System;

namespace VDCD.Common.DTOs
{
    public class CustomerResponse
    {
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Note { get; set; }
    }
}
