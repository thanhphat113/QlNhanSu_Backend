using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dapper;
using Microsoft.EntityFrameworkCore;
using QlNhanSu_Backend.DTO;
using QlNhanSu_Backend.Models;

namespace QlNhanSu_Backend.Services
{
    public interface IThongKeService
    {
        Task<dynamic> ThongKeTheoNgay(string NGAYBD, string NGAYKT);
    }
    public class ThongKeService : IThongKeService
    {
        private readonly QlNhanSuContext _context;
        private readonly IMapper _mapper;
        public ThongKeService(QlNhanSuContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<dynamic> ThongKeTheoNgay(string NGAYBD, string NGAYKT)
        {
            using (var connection = _context.Database.GetDbConnection())
            {
                var parameters = new { NGAYBD = NGAYBD, NGAYKT = NGAYKT };

                var items = await connection.QueryAsync<THONGKETHEONGAY>(
                    "THONGKETHEONGAY",
                    parameters,
                    commandType: System.Data.CommandType.StoredProcedure
                );

                var result = _mapper.Map<List<ThongKeTheoNgayDTO>>(items);

                return result;
            }
        }
    }
}