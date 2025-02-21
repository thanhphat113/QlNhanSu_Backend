using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QlNhanSu_Backend.DTO;
using QlNhanSu_Backend.Models;

namespace QlNhanSu_Backend.Services
{
	public interface IVaiTroService
	{
		Task<dynamic> GetAll();
		Task<dynamic> Them(ThemVT values);
		Task<dynamic> Xoa(int value);
		Task<dynamic> CapNhat(ThemVT values);
	}
	public class VaiTroService : IVaiTroService
	{
		private readonly QlNhanSuContext _context;
		public VaiTroService(QlNhanSuContext context)
		{
			_context = context;
		}

		public async Task<dynamic> CapNhat(ThemVT values)
		{
			var item = await _context.VaiTros.Include(v => v.IdQuyens).FirstOrDefaultAsync(u => u.IdVaiTro == values.IdVaiTro);

			var quyens = await _context.Quyens.AsNoTracking().ToListAsync();

			var currentPermissions = item.IdQuyens.Select(vq => vq.IdQuyen).ToList();

			var permissionsToAdd = values.SelectedId.Except(currentPermissions).ToList();
			var permissionsToRemove = currentPermissions.Except(values.SelectedId).ToList();

			foreach (var permissionId in permissionsToRemove)
			{
				var vaiTroQuyen = item.IdQuyens.FirstOrDefault(vq => vq.IdQuyen == permissionId);
				if (vaiTroQuyen != null)
				{
					item.IdQuyens.Remove(vaiTroQuyen);
				}
			}

			foreach (var permissionId in permissionsToAdd)
			{
				var vaiTroQuyen = new Quyen
				{
					TenQuyen = quyens.FirstOrDefault(q => q.IdQuyen == permissionId).TenQuyen,
					IdQuyen = permissionId
				};
				item.IdQuyens.Add(vaiTroQuyen);
			}

			try
			{
				var result = await _context.SaveChangesAsync();
				bool isSuccess = result > 0;
				return new
				{
					StatusCode = isSuccess ? 200 : 400,
					Data = new
					{
						vaiTro = isSuccess ? item : null,
						message = $"Cập nhật vai trò {(isSuccess ? "thành công" : "thất bại")}"
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
						message = "Lỗi thực hiện cập nhật vai trò: " + ex.Message
					}
				};
			}

		}

		public async Task<dynamic> GetAll()
		{
			return await _context.VaiTros.Include(u => u.IdQuyens).ToListAsync();
		}

		public async Task<dynamic> Them(ThemVT values)
		{
			var quyens = await _context.Quyens.ToListAsync();
			var VTs = await _context.VaiTros.FirstOrDefaultAsync(u => u.TenVaiTro.Equals(values.TenVaiTro));

			if (VTs != null) return new
			{
				StatusCode = 400,
				Data = new
				{
					message = "Tên vai trò đã hiện hữu"
				}
			};

			var Vt = new VaiTro
			{
				TenVaiTro = values.TenVaiTro,
				IdQuyens = quyens.Where(q => values.SelectedId.Contains(q.IdQuyen)).ToList()
			};

			try
			{
				await _context.VaiTros.AddAsync(Vt);
				var result = await _context.SaveChangesAsync();
				bool isSuccess = result > 0;
				return new
				{
					StatusCode = isSuccess ? 200 : 400,
					Data = new
					{
						vaiTro = isSuccess ? Vt : null,
						message = $"Thêm vai trò {(isSuccess ? "thành công" : "thất bại")}"
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
						message = "Lỗi thực hiện thêm vai trò: " + ex.Message
					}
				};
			}
		}

		public async Task<dynamic> Xoa(int value)
		{
			var item = await _context.VaiTros.FindAsync(value);

			try
			{
				if (item != null)
				{
					_context.VaiTros.Remove(item);
				}
				var result = await _context.SaveChangesAsync();
				bool isSuccess = result > 0;
				return new
				{
					StatusCode = isSuccess ? 200 : 400,
					Data = new
					{
						message = $"Xóa vai trò {(isSuccess ? "thành công" : "thất bại")}"
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
						message = "Lỗi thực hiện xóa vai trò: " + ex.Message
					}
				};
			}
		}
	}
}