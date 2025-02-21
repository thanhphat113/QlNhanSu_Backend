using System;
using System.Collections.Generic;

namespace QlNhanSu_Backend.Models;

public partial class TaiKhoanDangNhap
{
    public int IdMatKhau { get; set; }

    public string TaiKhoan { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? IdNhanVien { get; set; }

    public int? IdVaiTro { get; set; }

    public virtual NhanVien? IdNhanVienNavigation { get; set; }

    public virtual VaiTro? IdVaiTroNavigation { get; set; }
}
