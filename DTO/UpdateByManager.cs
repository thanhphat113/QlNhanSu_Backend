using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QlNhanSu_Backend.DTO
{
    public class UpdateByManager
    {
        public int IdNhanVien { get; set; }

        public string TenNv { get; set; }

        public string Sdt { get; set; }

        public string Email { get; set; }

        public int IdVaiTro { get; set; }
        public int IdChucVu { get; set; }
        public bool TrangThai { get; set; }
    }
}