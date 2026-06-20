using System.Threading.Tasks;
using VDCD.Common.Model;

namespace VDCD.BL.Interface
{
    public interface IBLUser : IBaseBL<User>
    {
        Task<User?> AuthenticateAsync(string userName, string password);
    }
}
