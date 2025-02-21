using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QlNhanSu_Backend.DTO
{
	public class UpdateTT
	{
		public int IdNhanVien { get; set; }

		public string TenNv { get; set; }

		public string Sdt { get; set; }

		public string Email { get; set; }

		public string? IdFingerPrint { get; set; } = null!;
	}
}