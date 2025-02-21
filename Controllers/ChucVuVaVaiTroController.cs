using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QlNhanSu_Backend.Services;

namespace QlNhanSu_Backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ChucVuVaVaiTroController : ControllerBase
	{
		private readonly IChucVuService _chucvu;
		private readonly IVaiTroService _vaitro;
		public ChucVuVaVaiTroController(IChucVuService chucvu, IVaiTroService vaitro)
		{
			_chucvu = chucvu;
			_vaitro = vaitro;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var vaitros = await _vaitro.GetAll();
			var chucvus = await _chucvu.GetAll();

			return Ok(new { chucvus = chucvus, vaitros = vaitros });
		}

		[HttpGet("{id}")]
		public ActionResult<string> Get(int id)
		{
			return "value";
		}

		[HttpPost]
		public void Post([FromBody] string value)
		{
		}

		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}