using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QlNhanSu_Backend.DTO;
using QlNhanSu_Backend.Services;

namespace QlNhanSu_Backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class VaiTroController : ControllerBase
	{
		private readonly IVaiTroService _vaitro;
		public VaiTroController(IVaiTroService vaitro)
		{
			_vaitro = vaitro;
		}
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			return Ok(await _vaitro.GetAll());
		}

		[HttpPost]
		public async Task<IActionResult> Get([FromBody] ThemVT values)
		{
			var result = await _vaitro.Them(values);
			// return Ok(result);
			return StatusCode(result.StatusCode, result.Data);
		}

		[HttpPut]
		public async Task<IActionResult> Update([FromBody] ThemVT values)
		{
			var result = await _vaitro.CapNhat(values);
			// return Ok(result);
			return StatusCode(result.StatusCode, result.Data);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			var result = await _vaitro.Xoa(id);
			// return Ok(result);
			return StatusCode(result.StatusCode, result.Data);
		}
	}
}