using System.Threading.Tasks;
using VDCD.BL.BaseBL;
using VDCD.Common.Model;
using VDCD.DL.Interface;
using VDCD.BL.Interface;

namespace VDCD.BL.Services
{
    public class BLUser : BaseBL<User>, IBLUser
    {
        private readonly IUserDL _userDL;

        public BLUser(IUserDL userDL) : base(userDL)
        {
            _userDL = userDL;
        }

        public async Task<User?> AuthenticateAsync(string userName, string password)
        {
            return await _userDL.GetUserByCredentialsAsync(userName, password);
        }
    }
}
