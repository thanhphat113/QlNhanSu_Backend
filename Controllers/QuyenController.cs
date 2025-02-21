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
    public class QuyenController : ControllerBase
    {
        private readonly IQuyenService _quyen;
        public QuyenController(IQuyenService quyen)
        {
            _quyen = quyen;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _quyen.GetAll());
        }
    }
}