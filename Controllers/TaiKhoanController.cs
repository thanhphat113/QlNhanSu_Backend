using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QlNhanSu_Backend.DTO;
using QlNhanSu_Backend.Models;
using QlNhanSu_Backend.Services;

namespace QlNhanSu_Backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class TaiKhoanController : ControllerBase
	{
		private readonly ITaiKhoanService _taikhoan;
		private readonly IConfiguration _configuration;

		public TaiKhoanController(IConfiguration configuration, ITaiKhoanService taikhoan)
		{
			_taikhoan = taikhoan;
			_configuration = configuration;
		}

		[HttpGet("Logout")]
		public async Task<IActionResult> Logout()
		{
			if (Request.Cookies.ContainsKey("access"))
			{
				Response.Cookies.Delete("access");
			}

			if (Request.Cookies.ContainsKey("refesh"))
			{
				Response.Cookies.Delete("refesh");
			}
			return Ok(true);
		}

		[HttpPut("update-device")]
		public async Task<IActionResult> Update([FromBody] UpdateDevice YeuCau)
		{
			var result = await _taikhoan.VerifyIdDevice(YeuCau.IdYeuCau);
			return StatusCode(result.statusCode, result.data);
		}

		[HttpPut("update-password")]
		public async Task<IActionResult> UpdatePass([FromBody] ChangePass values)
		{
			var userId = MiddleWare.GetUserIdFromCookie(Request);
			values.IdNhanVien = userId;
			var result = await _taikhoan.ChangePassword(values);
			// return Ok(result);
			return StatusCode(result.StatusCode, result.Data);
		}

		[AllowAnonymous]
		[HttpPost("Login")]
		public async Task<IActionResult> Login([FromBody] AccountUser values)
		{
			var login = await _taikhoan.Login(values);


			if (login.statusCode == 200)
			{
				var accessOptions = new CookieOptions
				{
					HttpOnly = true,
					SameSite = SameSiteMode.Strict,
					Secure = false,
					Expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpireMinutesAccess"])),
				};

				Response.Cookies.Append("access", login.access, accessOptions);
			}

			var result = new
			{
				statusCode = login.statusCode,
				data = login.data
			};

			return Ok(result);
		}
	}

	public class UpdateDevice
	{
		public int IdYeuCau { get; set; }
	}


}