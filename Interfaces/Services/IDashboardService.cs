using server.Dto;
using System.Data;

namespace server.Interfaces.Services
{
    public interface IDashboardService
    {
        Task<DataSet> GetAllDashboardDataAsync(string u, DashboardRequestDto r);
        Task<DataTable> GetKpisAsync(string u, DashboardRequestDto r);
    }
}