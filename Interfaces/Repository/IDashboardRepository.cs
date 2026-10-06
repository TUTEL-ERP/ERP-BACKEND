using server.Dto;
using server.Enums;
using System.Data;

namespace server.Interfaces.Repository
{
    public interface IDashboardRepository
    {
        Task<DataSet> ExecuteProcedureAsync(string usrname, DashboardAction action, DashboardRequestDto request);
    }
}