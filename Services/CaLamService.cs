using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QlNhanSu_Backend.Models;

namespace QlNhanSu_Backend.Services
{
	public interface ICaLamService
	{
		Task<dynamic> GetAll();
		Task<dynamic> Them(CaLam values);
		Task<dynamic> CapNhat(CaLam values);
		Task<dynamic> GetToCheckIn(int UserId);
		Task<dynamic> Xoa(int Id);
	}
	public class CaLamService : ICaLamService
	{
		private readonly QlNhanSuContext _context;
		public CaLamService(QlNhanSuContext context)
		{
			_context = context;
		}

		public async Task<dynamic> CapNhat(CaLam values)
		{
			var calam = await _context.CaLams.FindAsync(values.IdCaLam);
			calam.TenCaLam = values.TenCaLam;
			calam.ThoiGianBatDau = values.ThoiGianBatDau;
			calam.ThoiGianKetThuc = values.ThoiGianKetThuc;
			calam.ThoiGianSomChoPhep = values.ThoiGianSomChoPhep;
			calam.ThoiGianTreChoPhep = values.ThoiGianTreChoPhep;

			try
			{
				var result = await _context.SaveChangesAsync();
				bool isSuccess = result > 0;
				return new
				{
					StatusCode = isSuccess ? 200 : 400,
					Data = new
					{
						calam = isSuccess ? values : null,
						message = $"Cập nhật ca làm {(isSuccess ? "thành công" : "thất bại")}"
					}
				};
			}
			catch (Exception ex)
			{
				return new
				{
					StatusCode = 500,
					Data = new
					{
						message = "Lỗi thực hiện cập nhật ca làm: " + ex.Message
					}
				};
			}
		}

		public async Task<dynamic> GetAll()
		{
			return await _context.CaLams.ToListAsync();
		}

		public async Task<dynamic> GetToCheckIn(int UserId)
		{
			DateOnly dateOnly = DateOnly.FromDateTime(DateTime.Now);
			var diemdanhs = await _context.DiemDanhs
					.Where(c => c.NgayTao == dateOnly && c.IdNhanVien == UserId)
					.Select(c => c.IdCaLam)
					.ToListAsync();

			var result = await _context.CaLams
					.Where(c => !diemdanhs.Contains(c.IdCaLam))
					.ToListAsync();
			return result;
		}

		public async Task<dynamic> Them(CaLam values)
		{
			try
			{
				var haveCaLam = await _context.CaLams
					.Where(c => c.ThoiGianBatDau == values.ThoiGianBatDau &&
								c.ThoiGianKetThuc == values.ThoiGianKetThuc)
					.FirstOrDefaultAsync();

				if (haveCaLam != null) return new
				{
					StatusCode = 400,
					Data = new
					{
						message = "Khung giờ trên đã có một ca làm tương ứng"
					}
				};
				await _context.CaLams.AddAsync(values);
				var result = await _context.SaveChangesAsync();
				bool isSuccess = result > 0;
				return new
				{
					StatusCode = isSuccess ? 200 : 400,
					Data = new
					{
						calam = isSuccess ? values : null,
						message = $"Thêm ca làm {(isSuccess ? "thành công" : "thất bại")}"
					}
				};
			}
			catch (System.Exception ex)
			{
				return new
				{
					StatusCode = 500,
					Data = new
					{
						message = "Lỗi thực hiện thêm ca làm: " + ex.Message
					}
				};
			}
		}

		public async Task<dynamic> Xoa(int Id)
		{
			var calam = await _context.CaLams.FindAsync(Id);

			try
			{
				if (calam != null)
				{
					_context.CaLams.Remove(calam);
				}

				var result = await _context.SaveChangesAsync();
				bool isSuccess = result > 0;

				return new
				{
					StatusCode = isSuccess ? 200 : 400,
					Data = new
					{
						caLamId = Id,
						message = $"Xóa ca làm {(isSuccess ? "thành công" : "thất bại")}"
					}
				};
			}
			catch (Exception ex)
			{
				return new
				{
					StatusCode = 500,
					Data = new
					{
						message = "Lỗi thực hiện xóa ca làm: " + ex.Message
					}
				};
			}
		}
	}
}