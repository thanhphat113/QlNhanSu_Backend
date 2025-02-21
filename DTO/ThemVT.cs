using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QlNhanSu_Backend.DTO
{
	public class ThemVT
	{
		public int? IdVaiTro { get; set; } = null!;

		public string TenVaiTro { get; set; }
		public List<int> SelectedId { get; set; }
	}
}