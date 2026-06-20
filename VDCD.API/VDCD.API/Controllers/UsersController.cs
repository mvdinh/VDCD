using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VDCD.BL.Interface;
using VDCD.Common.DTOs;

namespace VDCD.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IBLUser _blUser;

        public UsersController(IBLUser blUser)
        {
            _blUser = blUser;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            try
            {
                var user = await _blUser.AuthenticateAsync(loginDto.UserName, loginDto.Password);
                
                if (user == null)
                {
                    return Unauthorized(new { Message = "Sai tên đăng nhập hoặc mật khẩu." });
                }

                // Protect password hash
                user.PasswordHash = string.Empty;

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}
