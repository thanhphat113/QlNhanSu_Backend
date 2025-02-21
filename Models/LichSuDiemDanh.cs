using System;
using System.Collections.Generic;

namespace QlNhanSu_Backend.Models;

public partial class LichSuDiemDanh
{
    public int IdLichSu { get; set; }

    public TimeOnly ThoiGianBatDau { get; set; }

    public double VaoTre { get; set; }

    public TimeOnly ThoiGianKetThuc { get; set; }

    public double VeSom { get; set; }

    public int IdDiemDanh { get; set; }

    public string GhiChu { get; set; } = null!;

    public DateTime ThoiGianCapNhat { get; set; }

    public int? IdQuanLy { get; set; }

    public virtual DiemDanh IdDiemDanhNavigation { get; set; } = null!;

    public virtual NhanVien? IdQuanLyNavigation { get; set; }
}
