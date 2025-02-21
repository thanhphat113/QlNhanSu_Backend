using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QlNhanSu_Backend.Models;

namespace QlNhanSu_Backend.DTO
{
	public class UserInfo
	{
		public int IdNhanVien { get; set; }

		public string TenNv { get; set; } = null!;

		public string Sdt { get; set; } = null!;

		public string Email { get; set; } = null!;

		public bool TrangThai { get; set; }
		public string? IdFingerPrint { get; set; } = null!;
		public bool? IsAccept { get; set; } = null!;

		public string ChucVu { get; set; }

		public ICollection<Quyen> Quyens { get; set; }
	}
}