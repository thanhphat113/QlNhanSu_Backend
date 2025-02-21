using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QlNhanSu_Backend.Models;

namespace QlNhanSu_Backend.DTO
{
	public class GetDD
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

		public CaLam IdCaLamNavigation { get; set; }
		public string TenQL { get; set; }
	}
}