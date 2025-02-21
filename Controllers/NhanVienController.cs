using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QlNhanSu_Backend.DTO;
using QlNhanSu_Backend.Services;

namespace QlNhanSu_Backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class NhanVienController : ControllerBase
	{
		private readonly INhanVienService _nhanvien;
		public NhanVienController(INhanVienService nhanvien)
		{
			_nhanvien = nhanvien;
		}

		[HttpPut]
		public async Task<IActionResult> UpdateTT([FromBody] UpdateTT values)
		{
			// return Ok(values);
			return Ok(await _nhanvien.UpdateInfo(values));
		}


		[HttpPost]
		public async Task<IActionResult> Post([FromBody] ThemNhanVien values)
		{
			var result = await _nhanvien.AddEmployee(values);
			// return Ok(result);
			return StatusCode(result.statusCode, result.data);
		}

		[HttpGet()]
		public async Task<IActionResult> Post(string value)
		{
			var result = await _nhanvien.GetInfoByPhoneOrEmail(value);
			return StatusCode(result.statusCode, result.data);
		}

		[HttpGet("count-user")]
		public async Task<IActionResult> CountUser()
		{
			var result = await _nhanvien.CountUserForDashboard();
			return Ok(result);
		}

		[HttpGet("user-for-select")]
		public async Task<IActionResult> GetForSelect()
		{
			var result = await _nhanvien.GetUserForSelect();
			return Ok(result);
		}

		[AllowAnonymous]
		[HttpGet("user-login")]
		public async Task<IActionResult> GetLoginUser()
		{
			var userId = MiddleWare.GetUserIdFromCookie(Request);
			var result = await _nhanvien.GetUserLogin(userId);
			return Ok(result);
		}

		[HttpGet("list-users-for-manager")]
		public async Task<IActionResult> GetListForManager()
		{
			return Ok(await _nhanvien.GetListForManager());
		}

		[HttpGet("user-detail")]
		public async Task<IActionResult> GetUserDetail(int IdNhanVien)
		{
			return Ok(await _nhanvien.GetByIdForUpdate(IdNhanVien));
		}

		[HttpPut("update-for-manager")]
		public async Task<IActionResult> Update([FromBody] UpdateByManager NhanVien)
		{
			var result = await _nhanvien.UpdateByManager(NhanVien);
			return StatusCode(result.StatusCode, result.Data);
		}

	}
}