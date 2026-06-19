using VDCD.Common.Model;
using VDCD.DL.Interface;
using VDCD.BL.BaseBL;

namespace VDCD.BL.Services
{
    public class BLUnit : BaseBL<Unit>
    {
        public BLUnit(IBaseDL<Unit> baseDL) : base(baseDL)
        {
        }
    }
}
