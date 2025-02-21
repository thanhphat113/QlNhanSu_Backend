
namespace QlNhanSu_Backend.DTO
{
    public class UserDetail
    {
        public int IdNhanVien { get; set; }

        public string TenNv { get; set; } = null!;

        public string Sdt { get; set; } = null!;

        public string Email { get; set; } = null!;

        public bool TrangThai { get; set; }
        public string? IdFingerPrint { get; set; } = null!;

        public int IdChucVu { get; set; }

        public int IdVaiTro { get; set; }
        public YeuCauDTO? YeuCau { get; set; }
    }
}