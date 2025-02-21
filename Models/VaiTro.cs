using System;
using System.Collections.Generic;

namespace QlNhanSu_Backend.Models;

public partial class VaiTro
{
    public int IdVaiTro { get; set; }

    public string TenVaiTro { get; set; } = null!;

    public virtual ICollection<TaiKhoanDangNhap> TaiKhoanDangNhaps { get; set; } = new List<TaiKhoanDangNhap>();

    public virtual ICollection<Quyen> IdQuyens { get; set; } = new List<Quyen>();
}
