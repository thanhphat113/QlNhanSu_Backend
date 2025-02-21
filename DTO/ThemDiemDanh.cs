using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QlNhanSu_Backend.DTO
{
	public class ThemDiemDanh
	{
		public int IdCaLam { get; set; }
		public int? IdNhanVien { get; set; } = null;
	}
}