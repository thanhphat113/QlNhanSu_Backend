using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QlNhanSu_Backend.DTO
{
    public class YeuCauDTO
    {
        public int IdYeuCau { get; set; }
        public bool TrangThai { get; set; }
        public UserForManager user { get; set; }
    }
}