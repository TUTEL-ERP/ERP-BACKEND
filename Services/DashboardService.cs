using server.Dto;
using server.Enums;
using server.Interfaces.Repository;
using server.Interfaces.Services;
using System.Data;

namespace server.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repo;
        private readonly ILogger<DashboardService> _logger;

        public DashboardService(IDashboardRepository repo, ILogger<DashboardService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<DataSet> GetAllDashboardDataAsync(string u, DashboardRequestDto r)
            => await _repo.ExecuteProcedureAsync(u, DashboardAction.ALL, r);

        public async Task<DataTable> GetKpisAsync(string u, DashboardRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, DashboardAction.KPIS, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }
    }
}