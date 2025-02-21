using System.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dapper;
using Microsoft.EntityFrameworkCore;
using QlNhanSu_Backend.DTO;
using QlNhanSu_Backend.Models;

namespace QlNhanSu_Backend.Services
{
	public interface IDiemDanhService
	{
		Task<dynamic> CheckIn(ThemDiemDanh values);
		Task<dynamic> CheckOut(int value, string ghichu);
		Task<dynamic> CheckStatus(int value);
		Task<dynamic> UpdateDD(UpdateDD update, int IdNhanVien);
		Task<dynamic> GetByUserId(int id);
		Task<dynamic> TiLeCheckIn();
		Task<dynamic> ThongKeCheckIn();
		Task<dynamic> DiemDanhChiTiet(int IdNhanVien, int IdCaLam);
	}
	public class DiemDanhService : IDiemDanhService
	{
		private readonly QlNhanSuContext _context;
		private readonly IMapper _mapper;
		public DiemDanhService(QlNhanSuContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<dynamic> CheckIn(ThemDiemDanh values)
		{
			var calam = await _context.CaLams.FindAsync(values.IdCaLam);
			var checkIn = _mapper.Map<DiemDanh>(values);
			var DateTimeNow = DateTime.Now;

			var time = DateTimeNow.TimeOfDay;
			var diff = calam.ThoiGianBatDau.ToTimeSpan() - time;
			if (diff.TotalMinutes > 15)
			{
				return new
				{
					statusCode = 400,
					data = new { message = "Chỉ có thể Check-In sớm tối đa 15 phút" }
				};
			}

			TimeSpan difference = DateTimeNow - DateTime.Today.Add(calam.ThoiGianBatDau.ToTimeSpan());
			var sumTime = difference.TotalMinutes;
			if (sumTime < 0)
			{
				checkIn.ThoiGianBatDau = calam.ThoiGianBatDau;
				checkIn.DanhGia = "Check-in đúng giờ";
				checkIn.HopLe = true;

			}
			else if (sumTime > calam.ThoiGianTreChoPhep)
			{
				checkIn.DanhGia = "Check-in trễ";
				checkIn.ThoiGianBatDau = TimeOnly.FromTimeSpan(time);
				checkIn.VaoTre = Math.Round(sumTime - calam.ThoiGianTreChoPhep);
				checkIn.HopLe = false;

			}


			try
			{
				await _context.DiemDanhs.AddAsync(checkIn);

				var result = await _context.SaveChangesAsync();

				checkIn.IdCaLamNavigation = calam;

				var rs = await _context.DiemDanhs
					.Where(u => u.IdDiemDanh == checkIn.IdDiemDanh)
					.Include(u => u.IdCaLamNavigation)
					.Include(u => u.IdQuanLyNavigation)
					.ProjectTo<GetDD>(_mapper.ConfigurationProvider)
					.FirstOrDefaultAsync();

				if (result > 0) return new
				{
					statusCode = 201,
					data = new
					{
						checkIn = rs,
						message = "Thực hiện điểm danh thành công"
					}
				};

				return new
				{
					statusCode = 400,
					data = new { message = "Thực hiện điểm danh thất bại" }
				};
			}
			catch (System.Exception ex)
			{
				return new
				{
					statusCode = 500,
					data = new
					{
						message = "Xảy ra lỗi trong quá trình thực hiện điểm danh",
						error = ex.Message
					}
				};
			}

		}

		public async Task<dynamic> CheckOut(int values, string ghichu)
		{
			var diemdanh = await _context.DiemDanhs
				.Where(u => u.IdDiemDanh == values)
				.Include(u => u.IdCaLamNavigation)
				.Include(u => u.IdQuanLyNavigation)
				.FirstOrDefaultAsync();

			var timeKT = DateTime.Now;
			var time = timeKT.TimeOfDay;

			if (TimeOnly.FromTimeSpan(time) < diemdanh.IdCaLamNavigation.ThoiGianBatDau)
			{
				return new
				{
					statusCode = 400,
					data = new
					{
						message = "Chưa thể thực hiện Check-out vì chưa vào thời gian bắt đầu ca",
					}
				};
			}

			var diff = time - diemdanh.ThoiGianBatDau.ToTimeSpan();
			if (diff.TotalMinutes < 60)
			{
				return new
				{
					statusCode = 400,
					data = new
					{
						message = "Chưa thể thực hiện Check-out vì bạn chưa làm đủ tối thiếu 1h/ca",
					}
				};
			}

			diemdanh.GhiChu = ghichu;
			TimeSpan difference = DateTime.Now.Add(diemdanh.IdCaLamNavigation.ThoiGianKetThuc.ToTimeSpan()) - timeKT;
			var sumTime = difference.TotalMinutes;

			if (sumTime < 0)
			{
				diemdanh.ThoiGianKetThuc = diemdanh.IdCaLamNavigation.ThoiGianKetThuc;
				diemdanh.DanhGia = diemdanh.DanhGia + " - " + "Check-out đúng giờ";
				if (!diemdanh.HopLe) diemdanh.HopLe = true;
			}
			else if (sumTime > diemdanh.IdCaLamNavigation.ThoiGianSomChoPhep)
			{
				diemdanh.ThoiGianKetThuc = TimeOnly.FromTimeSpan(time);
				diemdanh.DanhGia = diemdanh.DanhGia + " - " + "Check-out sớm";
				diemdanh.VeSom = Math.Round(sumTime - diemdanh.IdCaLamNavigation.ThoiGianSomChoPhep, 2);
				if (diemdanh.HopLe) diemdanh.HopLe = false;
			}

			try
			{
				var result = await _context.SaveChangesAsync();
				if (result > 0)
				{
					return new
					{
						statusCode = 200,
						data = new
						{
							message = "Checkout thành công",
							diemdanh = _mapper.Map<GetDD>(diemdanh)
						}
					};
				}

				return new
				{
					statusCode = 400,
					data = new
					{
						message = "Checkout không thành công"
					}
				};
			}
			catch (System.Exception ex)
			{
				return new
				{
					statusCode = 500,
					data = new
					{
						message = "Việc thực hiện checkout bị lỗi",
						error = ex.Message
					}
				};
			}
		}

		public async Task<dynamic> CheckStatus(int value)
		{
			var lastRecord = await _context.DiemDanhs
				.Where(d => d.IdNhanVien == value)
				.Include(d => d.IdCaLamNavigation)
				.OrderByDescending(d => d.ThoiGianBatDau)
				.FirstOrDefaultAsync();

			if (lastRecord == null || lastRecord.ThoiGianKetThuc != null)
			{
				return false;
			}

			return true;
		}

		public async Task<dynamic> DiemDanhChiTiet(int IdNhanVien, int IdCaLam)
		{
			using (var connection = _context.Database.GetDbConnection())
			{
				var parameters = new { IdNhanVien = IdNhanVien, IdCaLam = IdCaLam };
				var result = await connection.QueryAsync<DiemDanhChiTiet>(
					"DiemDanhChiTiet",
					parameters,
					commandType: CommandType.StoredProcedure
				);

				var calam = await _context.CaLams
						.Where(u => u.IdCaLam == IdCaLam)
						.ProjectTo<CaLamTK>(_mapper.ConfigurationProvider)
						.FirstOrDefaultAsync();


				return new { data = result, batDau = calam.BatDau, ketThuc = calam.KetThuc };
			}
		}

		public async Task<dynamic> GetByUserId(int id)
		{
			var checkIns = await _context.DiemDanhs
				.Where(u => u.IdNhanVien == id)
				.Include(u => u.IdCaLamNavigation)
				.Include(u => u.IdQuanLyNavigation)
				.ToListAsync();

			var lastRecord = await _context.DiemDanhs
				.Where(d => d.IdNhanVien == id)
				.OrderByDescending(d => d.IdDiemDanh)
				.FirstOrDefaultAsync();

			var result = _mapper.Map<List<GetDD>>(checkIns);

			if (lastRecord == null || lastRecord.ThoiGianKetThuc != null)
			{
				return new
				{
					lastCheckIn = new
					{
						idDiemDanh = lastRecord?.IdDiemDanh,
						hasCheckIn = false
					},
					results = result
				};
			}

			return new
			{
				lastCheckIn = new
				{
					idDiemDanh = lastRecord.IdDiemDanh,
					hasCheckIn = true
				},
				results = result
			};
		}

		public async Task<dynamic> ThongKeCheckIn()
		{
			using (var connection = _context.Database.GetDbConnection())
			{
				var result = await connection.QueryAsync<ThongKeDiemDanh>(
					"ThongKeDiemDanh",
					commandType: CommandType.StoredProcedure
				);
				return result;
			}
		}

		public async Task<dynamic> TiLeCheckIn()
		{
			using (var connection = _context.Database.GetDbConnection())
			{
				var parameters = new { CurrentDate = DateTime.Now.Date };
				var result = await connection.QueryFirstOrDefaultAsync<TiLeCheckInDungGio>(
				"TileCheckInDungGio",
				parameters,
				commandType: CommandType.StoredProcedure);
				return result;
			}
		}

		public async Task<dynamic> UpdateDD(UpdateDD update, int IdNhanVien)
		{
			var RoleId = await _context.NhanViens
				.AsNoTracking()
				.Where(a => a.IdNhanVien == IdNhanVien)
				.Select(a => a.IdChucVu)
			.FirstOrDefaultAsync();

			if (RoleId == 3) return new
			{
				statusCode = 400,
				data = new
				{
					message = "Bạn không có quyền thực hiện tác vụ"
				}
			};

			var diemdanh = await _context.DiemDanhs
				.Where(u => u.IdDiemDanh == update.IdDiemDanh)
				.Include(u => u.IdCaLamNavigation)
				.Include(u => u.IdQuanLyNavigation)
				.FirstOrDefaultAsync();

			if (diemdanh == null) return new
			{
				statusCode = 404,
				data = new { message = "Không thể tìm thấy Id hiện tại" }
			};

			var newLS = new LichSuDiemDanh
			{
				ThoiGianBatDau = diemdanh.ThoiGianBatDau,
				ThoiGianKetThuc = diemdanh.ThoiGianKetThuc.GetValueOrDefault(),
				IdDiemDanh = diemdanh.IdDiemDanh,
				GhiChu = diemdanh.GhiChu,
				ThoiGianCapNhat = diemdanh.ThoiGianCapNhat,
				VeSom = diemdanh.VeSom,
				VaoTre = diemdanh.VaoTre,
				IdQuanLy = diemdanh.IdQuanLy
			};

			using (var transaction = await _context.Database.BeginTransactionAsync())
			{
				try
				{
					await _context.LichSuDiemDanhs.AddAsync(newLS);
					DateTime today = DateTime.Today;


					diemdanh.ThoiGianBatDau = update.ThoiGianBatDau;
					diemdanh.ThoiGianKetThuc = update.ThoiGianKetThuc;
					diemdanh.GhiChu = update.GhiChu;
					diemdanh.IdQuanLy = IdNhanVien;

					var CaLam = diemdanh.IdCaLamNavigation;

					TimeSpan differenceBD = today.Add(update.ThoiGianBatDau.ToTimeSpan()) - today.Add(CaLam.ThoiGianBatDau.Add(TimeSpan.FromMinutes(CaLam.ThoiGianTreChoPhep)).ToTimeSpan());
					TimeSpan timeToSubtract = TimeSpan.FromMinutes(CaLam.ThoiGianSomChoPhep);

					TimeSpan differenceKT = today.Add(CaLam.ThoiGianKetThuc.ToTimeSpan()) - today.Add(update.ThoiGianKetThuc.ToTimeSpan());
					Console.WriteLine("Kết thúc: " + today.Add(CaLam.ThoiGianKetThuc.ToTimeSpan()) + " " + today.Add(update.ThoiGianKetThuc.ToTimeSpan()) + " " + differenceKT);
					TimeSpan subtract = differenceKT - timeToSubtract;
					var sumTimeBd = differenceBD.TotalMinutes;
					var sumTimeKt = subtract.TotalMinutes;


					diemdanh.VaoTre = Math.Round(sumTimeBd < 0 ? 0.0 : sumTimeBd, 2);
					diemdanh.VeSom = Math.Round(sumTimeKt < 0 ? 0.0 : sumTimeKt, 2);
					diemdanh.DanhGia = $"Check-In {(sumTimeBd < 0 ? "đúng giờ" : "trễ giờ")} - Check-Out {(sumTimeKt > 0 ? "sớm" : "đúng giờ")}";

					diemdanh.ThoiGianCapNhat = DateTime.UtcNow;


					await _context.SaveChangesAsync();

					await transaction.CommitAsync();

					return new
					{
						statusCode = 200,
						data = new
						{
							diemdanh = _mapper.Map<GetDD>(diemdanh),
							message = "Chỉnh sửa điểm danh thành công !"
						}
					};
				}
				catch (System.Exception ex)
				{
					await transaction.RollbackAsync();
					return new
					{
						statusCode = 500,
						data = new
						{
							message = "Chỉnh sửa thất bại",
							error = ex.Message
						}
					};
				}
			}

		}
	}
}