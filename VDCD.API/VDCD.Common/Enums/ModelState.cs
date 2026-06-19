using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VDCD.Common.Enums
{
    public enum ModelState : int
    {
        /// <summary>
        /// Không có trạng thái
        /// </summary>
        None = 0,

        /// <summary>
        /// Thêm 
        /// </summary>
        Insert = 1,

        /// <summary>
        /// sửa
        /// </summary>
        Update = 2,

        /// <summary>
        /// xóa
        /// </summary>
        Delete = 3
    }
}
