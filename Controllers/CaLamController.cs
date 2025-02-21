using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QlNhanSu_Backend.Models;
using QlNhanSu_Backend.Services;

namespace QlNhanSu_Backend.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class CaLamController : ControllerBase
	{
		private readonly ICaLamService _calam;

		public CaLamController(ICaLamService calam)
		{
			_calam = calam;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			return Ok(await _calam.GetAll());
		}

		[HttpGet("list-for-checkin")]
		public async Task<IActionResult> GetforCheckIn()
		{
			var userId = MiddleWare.GetUserIdFromCookie(Request);
			return Ok(await _calam.GetToCheckIn(userId));
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] CaLam values)
		{
			var result = await _calam.Them(values);
			return StatusCode(result.StatusCode, result.Data);
		}

		[HttpPut]
		public async Task<IActionResult> Put([FromBody] CaLam values)
		{
			var result = await _calam.CapNhat(values);
			return StatusCode(result.StatusCode, result.Data);
		}

		[HttpDelete("{idCaLam}")]
		public async Task<IActionResult> Delete(int idCaLam)
		{
			var result = await _calam.Xoa(idCaLam);
			return StatusCode(result.StatusCode, result.Data);
		}

	}
}