using System;
using System.Collections.Generic;

namespace QlNhanSu_Backend.Models;

public partial class YeuCauCapNhat
{
    public int IdYeuCau { get; set; }

    public int? IdNhanVien { get; set; }

    public string? GiaTriMoi { get; set; }

    public bool? TrangThai { get; set; }

    public virtual NhanVien? IdNhanVienNavigation { get; set; }
}
