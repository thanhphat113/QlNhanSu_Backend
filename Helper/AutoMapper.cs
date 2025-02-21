using AutoMapper;
using QlNhanSu_Backend.DTO;
using QlNhanSu_Backend.Models;

namespace Backend.Helper;
public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<ThemNhanVien, NhanVien>()
			.ForMember(dest => dest.TrangThai, opt => opt.MapFrom(src => true))
			.ReverseMap();
		CreateMap<NhanVien, UpdateTT>()
			.ForMember(dest => dest.IdFingerPrint, opt => opt.MapFrom(src => src.IdFingerPrint))
			.ReverseMap();

		CreateMap<DiemDanh, ThemDiemDanh>().ReverseMap();
		CreateMap<THONGKETHEONGAY, ThongKeTheoNgayDTO>()
			.ForMember(dest => dest.Times, opt => opt.MapFrom(src => ConvertToTimes(src.ChiTiet)))
		.ReverseMap();
		CreateMap<NhanVien, GetUserForSelect>().ReverseMap();
		CreateMap<ThemVT, VaiTro>()
			.ForMember(dest => dest.IdQuyens, opt => opt.MapFrom(src => ToList(src.SelectedId)))
			.ReverseMap();

		CreateMap<CaLam, CaLamTK>()
		.ForMember(dest => dest.BatDau, opt => opt.MapFrom(src => ConvertToDouble(src.ThoiGianBatDau)))
		.ForMember(dest => dest.KetThuc, opt => opt.MapFrom(src => ConvertToDouble(src.ThoiGianKetThuc)))
		.ReverseMap();
		// CreateMap<NhanVien, UpdateByManager>()
		// 	.ForMember(dest => dest.IdVaiTro, opt => opt.MapFrom(src => src.TaiKhoanDangNhap.IdVaiTro))
		// 	.ReverseMap();
		CreateMap<YeuCauCapNhat, YeuCauDTO>()
			.ForMember(dest => dest.user, opt => opt.MapFrom(src => src.IdNhanVienNavigation))
			.ReverseMap();
		CreateMap<NhanVien, UserForManager>()
			.ForMember(dest => dest.IdFingerPrint, opt => opt.MapFrom(src => GetFingerPrint(src)))
			.ForMember(dest => dest.ChucVu, opt => opt.MapFrom(src => src.IdChucVuNavigation.TenChucVu))
			.ForMember(dest => dest.IsWaiting, opt => opt.MapFrom(src => !src.YeuCauCapNhats.OrderByDescending(u => u.IdYeuCau).FirstOrDefault().TrangThai ?? false))
			.ReverseMap();
		CreateMap<NhanVien, UserInfo>()
			.ForMember(dest => dest.IdFingerPrint, opt => opt.MapFrom(src => GetFingerPrint(src)))
			.ForMember(dest => dest.ChucVu, opt => opt.MapFrom(src => src.IdChucVuNavigation.TenChucVu))
			.ForMember(dest => dest.Quyens, opt => opt.MapFrom(src => src.TaiKhoanDangNhap.IdVaiTroNavigation.IdQuyens))
			.ForMember(dest => dest.IsAccept, opt => opt.MapFrom(src => GetAccept(src.YeuCauCapNhats.OrderByDescending(u => u.IdYeuCau).FirstOrDefault())))
			.ReverseMap();

		CreateMap<VaiTro, VaiTroDTO>()
			.ReverseMap();

		CreateMap<DiemDanh, GetDD>()
			.ForMember(dest => dest.TenQL, opt => opt.MapFrom(src => src.IdQuanLyNavigation.TenNv))
			.ReverseMap();


		CreateMap<NhanVien, UserDetail>()
			.ForMember(dest => dest.IdFingerPrint, opt => opt.MapFrom(src => GetFingerPrint(src)))
			.ForMember(dest => dest.IdVaiTro, opt => opt.MapFrom(src => src.TaiKhoanDangNhap.IdVaiTroNavigation.IdVaiTro))
			.ForMember(dest => dest.YeuCau, opt => opt.MapFrom(src => src.YeuCauCapNhats.OrderByDescending(u => u.IdYeuCau).FirstOrDefault()))
			.ReverseMap();

	}

	private static string? GetFingerPrint(NhanVien values)
	{
		var lastYC = values.YeuCauCapNhats.OrderByDescending(u => u.IdYeuCau).FirstOrDefault(y => y.TrangThai == false || y.TrangThai == null);

		if (lastYC == null || lastYC.TrangThai.GetValueOrDefault()) return values.IdFingerPrint;
		return lastYC.GiaTriMoi;
	}

	private static List<ThoiGianLam> ConvertToTimes(string value)
	{
		if (value == null) return null;
		var items = value.Split(";");
		List<ThoiGianLam> result = new List<ThoiGianLam>();
		foreach (var item in items)
		{
			var attributes = item.Split(",");

			var newTG = new ThoiGianLam
			{
				Ngay = ConvertFormatDate(attributes[0].Split("/")[1]),
				ThoiGianCI = attributes[1].Split("/")[1],
				ThoiGianCO = attributes[2].Split("/")[1],
				CaLam = attributes[3].Split("/")[1],
				DanhGia = attributes[4].Split("/")[1],
			};

			result.Add(newTG);
		}
		return result;
	}

	private static string ConvertFormatDate(string value)
	{
		DateTime date = DateTime.ParseExact(value, "yyyy-MM-dd", null);
		string formattedDate = date.ToString("dd-MM-yyyy");
		return formattedDate;
	}

	private static double ConvertToDouble(TimeOnly value)
	{
		double timeInDouble = value.Hour + value.Minute / 60.0;

		// In ra kết quả
		return timeInDouble;
	}

	private static bool GetAccept(YeuCauCapNhat values)
	{
		if (values == null) return true;
		return values.TrangThai.GetValueOrDefault();
	}

	private static string? GetYeuCau(NhanVien values)
	{
		var lastYC = values.YeuCauCapNhats.OrderByDescending(u => u.IdYeuCau).FirstOrDefault();

		if (lastYC == null || lastYC.TrangThai.GetValueOrDefault()) return values.IdFingerPrint;
		return lastYC.GiaTriMoi;
	}

	private static List<Quyen> ToList(List<int> values)
	{
		List<Quyen> result = new List<Quyen>();
		foreach (var item in values)
		{
			var newI = new Quyen
			{
				IdQuyen = item
			};
			result.Add(newI);
		}

		return result;
	}
}
