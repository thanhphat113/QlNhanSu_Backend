using System;
using System.Collections.Generic;
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
	public class DiemDanhController : ControllerBase
	{
		private readonly IDiemDanhService _diemdanh;
		public DiemDanhController(IDiemDanhService diemdanh)
		{
			_diemdanh = diemdanh;
		}

		[HttpPost("check-in")]
		public async Task<IActionResult> Post([FromBody] ThemDiemDanh diemdanh)
		{
			var userId = MiddleWare.GetUserIdFromCookie(Request);
			diemdanh.IdNhanVien = userId;
			var result = await _diemdanh.CheckIn(diemdanh);
			return StatusCode(result.statusCode, result.data);
		}

		[HttpPut]
		public async Task<IActionResult> Put([FromBody] CheckOut values)
		{
			var result = await _diemdanh.CheckOut(values.IdDiemDanh, values.GhiChu);
			return StatusCode(result.statusCode, result.data);
		}

		[HttpPut("ChinhSuaDiemDanh")]
		public async Task<IActionResult> UpdateDD([FromBody] UpdateDD values)
		{
			var userId = MiddleWare.GetUserIdFromCookie(Request);
			var result = await _diemdanh.UpdateDD(values, userId);
			return StatusCode(result.statusCode, result.data);
		}

		[HttpGet("list-by-user-id")]
		public async Task<IActionResult> UpdateDD(int value)
		{
			return Ok(await _diemdanh.GetByUserId(value));
		}

		[HttpGet("ti-le-check-in")]
		public async Task<IActionResult> Tile()
		{
			return Ok(await _diemdanh.TiLeCheckIn());
		}

		[HttpGet("thong-ke-check-in")]
		public async Task<IActionResult> Thongke()
		{
			return Ok(await _diemdanh.ThongKeCheckIn());
		}

		[HttpGet("diem-danh-chi-tiet")]
		public async Task<IActionResult> ThongkeCT(int IdNhanVien, int IdCaLam)
		{
			return Ok(await _diemdanh.DiemDanhChiTiet(IdNhanVien, IdCaLam));
		}

		[HttpGet("check-status")]
		public async Task<IActionResult> CheckStatus(int value)
		{
			return Ok(await _diemdanh.CheckStatus(value));
		}

	}
}