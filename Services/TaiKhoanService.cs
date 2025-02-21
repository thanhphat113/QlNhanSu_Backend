using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QlNhanSu_Backend.DTO;
using QlNhanSu_Backend.Models;

namespace QlNhanSu_Backend.Services
{

	public interface ITaiKhoanService
	{
		Task<dynamic> Login(AccountUser values);
		Task<dynamic> PasswordRecoveryByPhone(string phone);
		Task<dynamic> PasswordRecoveryByEmail(string phone);

		Task<dynamic> VerifyIdDevice(int Id);
		Task<dynamic> ChangePassword(ChangePass values);

	}

	public class TaiKhoanService : ITaiKhoanService
	{
		private readonly QlNhanSuContext _context;

		private readonly IMapper _mapper;
		private readonly IJwtTokenService _jwt;
		private readonly INhanVienService _nhanvien;
		public TaiKhoanService(IMapper mapper, QlNhanSuContext context, IJwtTokenService jwt, INhanVienService nhanvien)
		{
			_context = context;
			_mapper = mapper;
			_jwt = jwt;
			_nhanvien = nhanvien;
		}

		public async Task<dynamic> ChangePassword(ChangePass values)
		{
			Console.WriteLine(values.IdNhanVien + values.NewPassword + values.OldPassword);
			var passwordHasher = new PasswordHasher<string>();


			var account = await _context.TaiKhoanDangNhaps.FindAsync(values.IdNhanVien);
			// return account;


			var verify = passwordHasher.VerifyHashedPassword(null, account.Password, values.OldPassword);

			if (verify == PasswordVerificationResult.Failed) return new
			{
				StatusCode = 400,
				Data = new
				{
					Message = "Mật khẩu cũ đã sai, vui lòng nhập lại"
				}
			};

			string hashedNewPassword = passwordHasher.HashPassword(null, values.NewPassword);

			account.Password = hashedNewPassword;

			try
			{
				var result = await _context.SaveChangesAsync();
				var isSuccess = result > 0;
				return new
				{
					StatusCode = isSuccess ? 200 : 400,
					Data = new
					{
						Message = $"Cập nhật mật khẩu {(isSuccess ? "thành công" : "thất bại")}"
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
						Message = ex.Message
					}
				};
			}

		}

		public async Task<dynamic> Login(AccountUser values)
		{
			var account = await _context.TaiKhoanDangNhaps
				.Where(a => a.TaiKhoan.Equals(values.username))
				.Include(a => a.IdNhanVienNavigation)
				.FirstOrDefaultAsync(a => a.IdNhanVienNavigation.TrangThai == true);



			if (account == null || !string.Equals(account.TaiKhoan, values.username, StringComparison.Ordinal)) return new
			{
				statusCode = 400,
				data = new
				{
					message = "Tên tài khoản không tồn tại"
				}
			};


			var passwordHasher = new PasswordHasher<string>();

			var result = passwordHasher.VerifyHashedPassword(null, account.Password, values.password);

			if (result == PasswordVerificationResult.Success)
			{
				var user = await _context.NhanViens.Where(u => u.IdNhanVien == account.IdNhanVien)
						.Include(u => u.IdChucVuNavigation)
						.Include(q => q.TaiKhoanDangNhap)
						.ThenInclude(q => q.IdVaiTroNavigation)
						.ThenInclude(v => v.IdQuyens)
						.Include(u => u.YeuCauCapNhats)
						.ProjectTo<UserInfo>(_mapper.ConfigurationProvider)
						.FirstOrDefaultAsync();

				return new
				{
					statusCode = 200,
					data = new
					{
						information = user,
						message = "Đăng nhập thành công",

					},
					access = _jwt.GenerateJwtToken(account.IdNhanVien.ToString()),
				};
			}

			return new
			{
				statusCode = 401,
				data = new
				{
					message = "Sai mật khẩu, vui lòng nhập lại !!!"
				}
			};

		}


		public Task<dynamic> PasswordRecoveryByEmail(string phone)
		{
			throw new NotImplementedException();
		}

		public Task<dynamic> PasswordRecoveryByPhone(string phone)
		{
			throw new NotImplementedException();
		}

		public async Task<dynamic> VerifyIdDevice(int Id)
		{
			var yeucau = await _context.YeuCauCapNhats.FindAsync(Id);

			yeucau.TrangThai = true;

			var user = await _context.NhanViens.FindAsync(yeucau.IdNhanVien);
			user.IdFingerPrint = yeucau.GiaTriMoi;

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
							YeuCau = _mapper.Map<YeuCauDTO>(yeucau),
							message = "Xác nhận thiết bị mới thành công !"
						}
					};
				}

				return new
				{
					statusCode = 400,
					data = new
					{
						message = "Xác nhận thiết bị mới thất bại !"
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
						message = "Lỗi việc xác thực thiết bị mới: " + ex.Message
					}
				};
			}
		}
	}

}