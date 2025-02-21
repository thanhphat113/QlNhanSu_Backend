using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QlNhanSu_Backend.Models;

namespace QlNhanSu_Backend.Services
{
	public interface IQuyenService
	{
		Task<dynamic> GetAll();
	}
	public class QuyenService : IQuyenService
	{
		private readonly QlNhanSuContext _context;
		public QuyenService(QlNhanSuContext context)
		{
			_context = context;
		}
		public async Task<dynamic> GetAll()
		{
			return await _context.Quyens.ToListAsync();
		}
	}
}