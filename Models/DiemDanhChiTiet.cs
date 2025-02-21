using System;

namespace QlNhanSu_Backend.Models
{
	public class DiemDanhChiTiet
	{
		public DateTime? WeekDay { get; set; }
		public double? ThoiGianBatDau { get; set; }
		public double? ThoiGianKetThuc { get; set; }
		public bool HopLe { get; set; }
	}
}
