using VDCD.Common.Model;
using VDCD.DL.Interface;
using VDCD.BL.BaseBL;

namespace VDCD.BL.Services
{
    public class BLCategory : BaseBL<Category>
    {
        public BLCategory(IBaseDL<Category> baseDL) : base(baseDL)
        {
        }
    }
}
