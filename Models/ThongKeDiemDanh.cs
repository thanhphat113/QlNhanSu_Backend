using System;

namespace QlNhanSu_Backend.Models
{
	public class ThongKeDiemDanh
	{
		public DateTime? WeekDay { get; set; }
		public int? CheckInDungGio { get; set; }
		public int? CheckInTre { get; set; }
		public int? CheckOutDungGio { get; set; }
		public int? CheckOutSom { get; set; }
	}
}
