using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QlNhanSu_Backend.DTO
{
	public class ThongKeTheoNgayDTO
	{
		public int IdNhanVien { get; set; }
		public string TenNv { get; set; }
		public ICollection<ThoiGianLam>? Times { get; set; }
	}
}