using System;
using System.Collections.Generic;

namespace QlNhanSu_Backend.Models;

public partial class DiemDanh
{
    public int IdDiemDanh { get; set; }

    public TimeOnly ThoiGianBatDau { get; set; }

    public double VaoTre { get; set; }

    public TimeOnly? ThoiGianKetThuc { get; set; }

    public double VeSom { get; set; }

    public string? DanhGia { get; set; }

    public string? GhiChu { get; set; }

    public DateOnly NgayTao { get; set; }

    public DateTime ThoiGianCapNhat { get; set; }

    public int IdCaLam { get; set; }

    public int IdNhanVien { get; set; }

    public int? IdQuanLy { get; set; }

    public bool HopLe { get; set; }

    public virtual CaLam IdCaLamNavigation { get; set; } = null!;

    public virtual NhanVien IdNhanVienNavigation { get; set; } = null!;

    public virtual NhanVien? IdQuanLyNavigation { get; set; }

    public virtual ICollection<LichSuDiemDanh> LichSuDiemDanhs { get; set; } = new List<LichSuDiemDanh>();
}
