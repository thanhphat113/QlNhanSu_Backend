using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QlNhanSu_Backend.DTO
{
	public class UpdateDD
	{
		public int IdDiemDanh { get; set; }
		public TimeOnly ThoiGianBatDau { get; set; }
		public TimeOnly ThoiGianKetThuc { get; set; }
		public string GhiChu { get; set; }

	}
}