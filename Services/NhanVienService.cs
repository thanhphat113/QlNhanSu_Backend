using System.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QlNhanSu_Backend.DTO;
using QlNhanSu_Backend.Models;

namespace QlNhanSu_Backend.Services
{
	public interface INhanVienService
	{
		Task<dynamic> AddEmployee(ThemNhanVien values);
		Task<dynamic> UpdateInfo(UpdateTT values);
		Task<dynamic> UpdateByManager(UpdateByManager values);
		Task<dynamic> GetInfoByPhoneOrEmail(string value);
		Task<dynamic> GetListForManager();
		Task<dynamic> CountUserForDashboard();
		Task<dynamic> GetByIdForUpdate(int Id);
		Task<dynamic> GetById(int Id);
		Task<dynamic> GetUserLogin(int Id);
		Task<dynamic> GetUserForSelect();
	}
	public class NhanVienService : INhanVienService
	{
		private readonly QlNhanSuContext _context;
		private readonly IMapper _mapper;
		public NhanVienService(QlNhanSuContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<dynamic> AddEmployee(ThemNhanVien values)
		{
			var passwordHasher = new PasswordHasher<string>();
			string hashedPassword = passwordHasher.HashPassword(null, "123456");

			var newEmployee = _mapper.Map<NhanVien>(values);


			var newAccount = new TaiKhoanDangNhap
			{
				Password = hashedPassword,
				TaiKhoan = values.Email.Split('@')[0],
				IdVaiTro = values.IdVaiTro
			};

			newEmployee.TaiKhoanDangNhap = newAccount;


			await _context.NhanViens.AddAsync(newEmployee);
			try
			{
				var result = await _context.SaveChangesAsync();

				var newUser = await GetById(newEmployee.IdNhanVien);

				var response = _mapper.Map<UserForManager>(newUser);

				if (result > 0)
				{
					return new
					{
						statusCode = 201,
						data = new
						{
							message = "Thêm nhân viên thành công!",
							user = response
						}
					};
				}

				return new
				{
					statusCode = 400,
					data = new
					{
						message = "Thêm nhân viên thất bại!",
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
						message = ex.Data,
					}
				};
			}

		}

		public async Task<dynamic> GetById(int Id)
		{
			return await _context.NhanViens
				.Where(u => u.IdNhanVien == Id)
				.Include(u => u.IdChucVuNavigation)
				.Include(q => q.TaiKhoanDangNhap)
				.ThenInclude(q => q.IdVaiTroNavigation)
				.ThenInclude(v => v.IdQuyens)
				.Include(u => u.YeuCauCapNhats)
				.FirstOrDefaultAsync();
		}

		public async Task<dynamic> GetByIdForUpdate(int Id)
		{
			var user = await GetById(Id);
			if (user == null) return null;

			var result = _mapper.Map<UserDetail>(user);
			return result;
		}

		public async Task<dynamic> GetInfoByPhoneOrEmail(string value)
		{
			var user = await _context.NhanViens
				.Where(u => u.Sdt == value ||
					u.Email == value)
				.FirstOrDefaultAsync();

			if (user == null) return new
			{
				statusCode = 400,
				data = new { message = "Số điện thoại hoặc email bạn cung cấp không hợp lệ" }
			};

			return new
			{
				statusCode = 200,
				data = user
			};
		}

		public async Task<dynamic> CountUserForDashboard()
		{
			using (var connection = _context.Database.GetDbConnection())
			{

				var result = await connection.QueryFirstOrDefaultAsync<SoLuongNV>(
				"SoLuongNV",
				commandType: CommandType.StoredProcedure);

				return result;
			}
		}

		public async Task<dynamic> GetListForManager()
		{
			return await _context.NhanViens
					.Where(u => u.IdChucVu == 3 || u.IdChucVu == 4)
					.Include(u => u.YeuCauCapNhats)
					.ProjectTo<UserForManager>(_mapper.ConfigurationProvider)
					.ToListAsync();
		}

		public async Task<dynamic> GetUserLogin(int Id)
		{
			var user = await GetById(Id);
			if (user == null) return null;

			// return user;

			var result = _mapper.Map<UserInfo>(user);
			return result;
		}

		public async Task<dynamic> UpdateByManager(UpdateByManager values)
		{
			var nhanvien = await _context.NhanViens
				.Where(u => u.IdNhanVien == values.IdNhanVien)
				.Include(u => u.TaiKhoanDangNhap)
				.FirstOrDefaultAsync();

			nhanvien.Email = values.Email;
			nhanvien.Sdt = values.Sdt;
			nhanvien.TrangThai = values.TrangThai;
			// nhanvien.IdChucVu = values.IdChucVu;
			nhanvien.TenNv = values.TenNv;
			nhanvien.TaiKhoanDangNhap.IdVaiTro = values.IdVaiTro;


			var ChucVu = await _context.ChucVus.FindAsync(values.IdChucVu);

			nhanvien.IdChucVuNavigation = ChucVu;

			try
			{
				var result = await _context.SaveChangesAsync();
				bool isSuccess = result > 0;
				return new
				{
					StatusCode = isSuccess ? 200 : 400,
					Data = new
					{
						user = isSuccess ? _mapper.Map<UserForManager>(nhanvien) : null,
						message = $"Cập nhật thông tin {(isSuccess ? "thành công" : "thất bại")}"
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
						message = "Lỗi thực hiện cập nhật " + ex.Message
					}
				};
			}
		}

		public async Task<dynamic> UpdateInfo(UpdateTT values)
		{
			try
			{
				var employee = await GetById(values.IdNhanVien);

				if (employee == null) return new
				{
					statusCode = 404,
					message = "Không tìm thấy Id người dùng"
				};

				employee.Sdt = values.Sdt;
				employee.TenNv = values.TenNv;
				employee.Email = values.Email;

				if (employee.IdChucVu == 3)
				{
					if (employee.IdFingerPrint != values.IdFingerPrint)
					{
						var item = new YeuCauCapNhat
						{
							IdNhanVien = values.IdNhanVien,
							GiaTriMoi = values.IdFingerPrint,
						};

						await _context.YeuCauCapNhats.AddAsync(item);
					}
				}
				else
				{
					employee.IdFingerPrint = values.IdFingerPrint;
				}

				var result = _mapper.Map<UserInfo>(employee);

				await _context.SaveChangesAsync();
				return new
				{
					StatusCode = 200,
					data = new
					{
						information = result,
						Message = "Cập nhật thông tin thành công"
					}
				};
			}
			catch (Exception ex)
			{
				return new
				{
					StatusCode = 500,
					data = new
					{
						Message = "Cập nhật thông tin bị lỗi",
						Error = ex.Message
					}
				}
			;
			}
		}

		public async Task<dynamic> GetUserForSelect()
		{
			return await _context.NhanViens
				.Where(u => u.IdChucVu == 3 || u.IdChucVu == 4)
				.ProjectTo<GetUserForSelect>(_mapper.ConfigurationProvider)
				.ToListAsync();
		}
	}
}