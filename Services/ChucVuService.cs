using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QlNhanSu_Backend.Models;

namespace QlNhanSu_Backend.Services
{
	public interface IChucVuService
	{
		Task<dynamic> GetAll();
	}

	public class ChucVuService : IChucVuService
	{
		private readonly QlNhanSuContext _context;
		public ChucVuService(QlNhanSuContext context)
		{
			_context = context;
		}
		public async Task<dynamic> GetAll()
		{
			return await _context.ChucVus.ToListAsync();
		}
	}
}