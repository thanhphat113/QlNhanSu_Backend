using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QlNhanSu_Backend.Services;

namespace QlNhanSu_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ThongKeController : ControllerBase
    {
        private readonly IThongKeService _thongke;
        public ThongKeController(IThongKeService thongke)
        {
            _thongke = thongke;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string NgayBd, string NgayKt)
        {
            var result = await _thongke.ThongKeTheoNgay(NgayBd, NgayKt);
            return Ok(result);
        }
    }
}