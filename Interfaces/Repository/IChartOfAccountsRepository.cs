using server.Dto;
using server.Enums;
using System.Data;

namespace server.Interfaces.Repository
{
    public interface IChartOfAccountsRepository
    {
        Task<DataSet> ExecuteProcedureAsync(string usrname, ChartOfAccountsAction action, ChartOfAccountsRequestDto request);
    }
}