using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QlNhanSu_Backend.Models;

namespace QlNhanSu_Backend.DTO
{
	public class ThemNhanVien
	{
		public string TenNv { get; set; } = null!;

		public string Sdt { get; set; } = null!;

		public string Email { get; set; } = null!;

		public int IdChucVu { get; set; }
		public int IdVaiTro { get; set; }
	}
}