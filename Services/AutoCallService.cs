using Microsoft.EntityFrameworkCore;
using QlNhanSu_Backend.Models;

namespace QlNhanSu_Backend.Services
{
	public interface IAutoCallService
	{
		Task CallApi();

	}
	public class AutoCallService : IAutoCallService
	{
		private readonly QlNhanSuContext _context;
		public AutoCallService(QlNhanSuContext context)
		{
			_context = context;
		}

		public async Task CallApi()
		{
			var diemdanhs = await _context.DiemDanhs
					.Where(d => d.ThoiGianKetThuc == null)
					.ToListAsync();

			foreach (var item in diemdanhs)
			{
				item.ThoiGianKetThuc = TimeOnly.MaxValue;
				item.DanhGia = item.DanhGia + " - " + "Quên check-out";
				item.HopLe = false;
			}

			await _context.SaveChangesAsync();
		}
	}
}