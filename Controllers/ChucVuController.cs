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
	public class ChucVuController : ControllerBase
	{
		private readonly IChucVuService _chucvu;
		public ChucVuController(IChucVuService chucvu)
		{
			_chucvu = chucvu;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			return Ok(await _chucvu.GetAll());
		}
	}
}