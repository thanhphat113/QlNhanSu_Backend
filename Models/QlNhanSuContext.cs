using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QlNhanSu_Backend.Models;

public partial class QlNhanSuContext : DbContext
{
    public QlNhanSuContext()
    {
    }

    public QlNhanSuContext(DbContextOptions<QlNhanSuContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CaLam> CaLams { get; set; }

    public virtual DbSet<ChucVu> ChucVus { get; set; }

    public virtual DbSet<DiemDanh> DiemDanhs { get; set; }

    public virtual DbSet<LichSuDiemDanh> LichSuDiemDanhs { get; set; }

    public virtual DbSet<NhanVien> NhanViens { get; set; }

    public virtual DbSet<Quyen> Quyens { get; set; }

    public virtual DbSet<TaiKhoanDangNhap> TaiKhoanDangNhaps { get; set; }

    public virtual DbSet<VaiTro> VaiTros { get; set; }

    public virtual DbSet<YeuCauCapNhat> YeuCauCapNhats { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=QlNhanSu;User Id=sa;Password=123456aA@$;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CaLam>(entity =>
        {
            entity.HasKey(e => e.IdCaLam).HasName("PK__tmp_ms_x__7426907FB532E8C4");

            entity.ToTable("CaLam");

            entity.HasIndex(e => e.IdCaLam, "UQ__tmp_ms_x__7426907E45BA9F44").IsUnique();

            entity.Property(e => e.TenCaLam).HasMaxLength(255);
        });

        modelBuilder.Entity<ChucVu>(entity =>
        {
            entity.HasKey(e => e.IdChucVu).HasName("PK__ChucVu__81D916DF5EE0C1F3");

            entity.ToTable("ChucVu");

            entity.HasIndex(e => e.IdChucVu, "UQ__ChucVu__81D916DEA0BDAC32").IsUnique();

            entity.Property(e => e.TenChucVu).HasMaxLength(255);
        });

        modelBuilder.Entity<DiemDanh>(entity =>
        {
            entity.HasKey(e => e.IdDiemDanh).HasName("PK__tmp_ms_x__6C04A5D9A3A78B91");

            entity.ToTable("DiemDanh");

            entity.HasIndex(e => e.IdDiemDanh, "UQ__tmp_ms_x__6C04A5D8326DACBE").IsUnique();

            entity.Property(e => e.DanhGia).HasMaxLength(50);
            entity.Property(e => e.NgayTao).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ThoiGianCapNhat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ThoiGianKetThuc).HasDefaultValueSql("(NULL)");

            entity.HasOne(d => d.IdCaLamNavigation).WithMany(p => p.DiemDanhs)
                .HasForeignKey(d => d.IdCaLam)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DiemDanh__IdCaLa__42E1EEFE");

            entity.HasOne(d => d.IdNhanVienNavigation).WithMany(p => p.DiemDanhIdNhanVienNavigations)
                .HasForeignKey(d => d.IdNhanVien)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DiemDanh__IdNhan__32AB8735");

            entity.HasOne(d => d.IdQuanLyNavigation).WithMany(p => p.DiemDanhIdQuanLyNavigations)
                .HasForeignKey(d => d.IdQuanLy)
                .HasConstraintName("FK__DiemDanh__IdQuan__339FAB6E");
        });

        modelBuilder.Entity<LichSuDiemDanh>(entity =>
        {
            entity.HasKey(e => e.IdLichSu).HasName("PK__tmp_ms_x__823B1772D80F2E00");

            entity.ToTable("LichSuDiemDanh");

            entity.HasIndex(e => e.IdLichSu, "UQ__tmp_ms_x__823B1773C4B609D2").IsUnique();

            entity.Property(e => e.GhiChu).HasMaxLength(255);
            entity.Property(e => e.ThoiGianCapNhat).HasColumnType("datetime");

            entity.HasOne(d => d.IdDiemDanhNavigation).WithMany(p => p.LichSuDiemDanhs)
                .HasForeignKey(d => d.IdDiemDanh)
                .HasConstraintName("FK__LichSuDie__IdDie__3864608B");

            entity.HasOne(d => d.IdQuanLyNavigation).WithMany(p => p.LichSuDiemDanhs)
                .HasForeignKey(d => d.IdQuanLy)
                .HasConstraintName("FK__LichSuDie__IdQua__395884C4");
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => e.IdNhanVien).HasName("PK__tmp_ms_x__B8294845362C007C");

            entity.ToTable("NhanVien");

            entity.HasIndex(e => e.IdNhanVien, "UQ__tmp_ms_x__B82948449B11EEA1").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.IdFingerPrint)
                .IsUnicode(false)
                .HasDefaultValueSql("(NULL)");
            entity.Property(e => e.Sdt)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("SDT");
            entity.Property(e => e.TenNv)
                .HasMaxLength(255)
                .HasColumnName("TenNV");
            entity.Property(e => e.TrangThai).HasDefaultValue(true);

            entity.HasOne(d => d.IdChucVuNavigation).WithMany(p => p.NhanViens)
                .HasForeignKey(d => d.IdChucVu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NhanVien__IdChuc__7B5B524B");
        });

        modelBuilder.Entity<Quyen>(entity =>
        {
            entity.HasKey(e => e.IdQuyen).HasName("PK__Quyen__C9FD630AC4EDF64C");

            entity.ToTable("Quyen");

            entity.HasIndex(e => e.IdQuyen, "UQ__Quyen__C9FD630B60CC7EB4").IsUnique();

            entity.Property(e => e.TenQuyen).HasMaxLength(255);
        });

        modelBuilder.Entity<TaiKhoanDangNhap>(entity =>
        {
            entity.HasKey(e => e.IdMatKhau).HasName("PK__tmp_ms_x__702FCEBA8F8AE986");

            entity.ToTable("TaiKhoanDangNhap");

            entity.HasIndex(e => e.IdNhanVien, "UC_IdNhanVien").IsUnique();

            entity.HasIndex(e => e.IdMatKhau, "UQ__tmp_ms_x__702FCEBBD6E25995").IsUnique();

            entity.HasIndex(e => e.IdNhanVien, "UQ__tmp_ms_x__B82948444D3C3DF8").IsUnique();

            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TaiKhoan)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.IdNhanVienNavigation).WithOne(p => p.TaiKhoanDangNhap)
                .HasForeignKey<TaiKhoanDangNhap>(d => d.IdNhanVien)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__TaiKhoanD__IdNha__282DF8C2");

            entity.HasOne(d => d.IdVaiTroNavigation).WithMany(p => p.TaiKhoanDangNhaps)
                .HasForeignKey(d => d.IdVaiTro)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_TK_VaiTroId_VaiTro");
        });

        modelBuilder.Entity<VaiTro>(entity =>
        {
            entity.HasKey(e => e.IdVaiTro);

            entity.ToTable("VaiTro");

            entity.Property(e => e.TenVaiTro).HasMaxLength(50);

            entity.HasMany(d => d.IdQuyens).WithMany(p => p.IdVaiTros)
                .UsingEntity<Dictionary<string, object>>(
                    "VaiTroQuyen",
                    r => r.HasOne<Quyen>().WithMany()
                        .HasForeignKey("IdQuyen")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_QuyenId_Quyen"),
                    l => l.HasOne<VaiTro>().WithMany()
                        .HasForeignKey("IdVaiTro")
                        .HasConstraintName("FK_VaiTroId_VaiTro"),
                    j =>
                    {
                        j.HasKey("IdVaiTro", "IdQuyen").HasName("PK_ChucVuQuyen");
                        j.ToTable("VaiTroQuyen");
                    });
        });

        modelBuilder.Entity<YeuCauCapNhat>(entity =>
        {
            entity.HasKey(e => e.IdYeuCau);

            entity.ToTable("YeuCauCapNhat");

            entity.Property(e => e.GiaTriMoi).IsUnicode(false);
            entity.Property(e => e.TrangThai).HasDefaultValue(false);

            entity.HasOne(d => d.IdNhanVienNavigation).WithMany(p => p.YeuCauCapNhats)
                .HasForeignKey(d => d.IdNhanVien)
                .HasConstraintName("FK_nhanvien_YeuCauCapNhat");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
