using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QlNhanSu_Backend.DTO
{
    public class ChangePass
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string RePassword { get; set; }
        public int? IdNhanVien { get; set; } = null;
    }
}