using System.Threading.Tasks;
using VDCD.Common.Model;

namespace VDCD.DL.Interface
{
    public interface IUserDL : IBaseDL<User>
    {
        Task<User?> GetUserByCredentialsAsync(string userName, string password);
    }
}
