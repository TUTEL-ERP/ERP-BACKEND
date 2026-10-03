using server.Dto;
using System.Data;

namespace server.Interfaces.Services
{
    public interface IChartOfAccountsService
    {
        Task<DataTable> GetGridDataAsync(string u, ChartOfAccountsRequestDto r);
        Task<DataTable> GetSingleRecordAsync(string u, ChartOfAccountsRequestDto r);
        Task<DataTable> InsertRecordAsync(string u, ChartOfAccountsRequestDto r);
        Task<DataTable> UpdateRecordAsync(string u, ChartOfAccountsRequestDto r);
        Task<DataTable> DeleteRecordAsync(string u, ChartOfAccountsRequestDto r);
        Task<DataTable> GetParentAccountsAsync(string u, ChartOfAccountsRequestDto r);
        Task<DataTable> GetNextAccountCodeAsync(string u, ChartOfAccountsRequestDto r);
        Task<DataTable> GetTreeAsync(string u, ChartOfAccountsRequestDto r);
    }
}