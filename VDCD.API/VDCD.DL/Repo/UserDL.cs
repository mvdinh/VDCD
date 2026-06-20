using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using VDCD.Common.Model;
using VDCD.DL.Base;
using VDCD.DL.ConnectDB;
using VDCD.DL.Interface;

namespace VDCD.DL.Repo
{
    public class UserDL : BaseDL<User>, IUserDL
    {
        private readonly DBContext _dbContext;

        public UserDL(DBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetUserByCredentialsAsync(string userName, string password)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == userName && u.PasswordHash == password);
        }
    }
}
