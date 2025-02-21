using System;
using System.Collections.Generic;

namespace QlNhanSu_Backend.Models;

public partial class NhanVien
{
    public int IdNhanVien { get; set; }

    public string TenNv { get; set; } = null!;

    public string Sdt { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool TrangThai { get; set; }

    public int IdChucVu { get; set; }

    public string? IdFingerPrint { get; set; }

    public virtual ICollection<DiemDanh> DiemDanhIdNhanVienNavigations { get; set; } = new List<DiemDanh>();

    public virtual ICollection<DiemDanh> DiemDanhIdQuanLyNavigations { get; set; } = new List<DiemDanh>();

    public virtual ChucVu IdChucVuNavigation { get; set; } = null!;

    public virtual ICollection<LichSuDiemDanh> LichSuDiemDanhs { get; set; } = new List<LichSuDiemDanh>();

    public virtual TaiKhoanDangNhap? TaiKhoanDangNhap { get; set; }

    public virtual ICollection<YeuCauCapNhat> YeuCauCapNhats { get; set; } = new List<YeuCauCapNhat>();
}
