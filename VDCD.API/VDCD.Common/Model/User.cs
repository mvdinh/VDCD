using System;
using VDCD.Common.Base;

namespace VDCD.Common.Model
{
    public class User : BaseEntity
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "Staff";
    }
}
