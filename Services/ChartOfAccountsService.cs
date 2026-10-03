using server.Dto;
using server.Enums;
using server.Interfaces.Repository;
using server.Interfaces.Services;
using System.Data;

namespace server.Services
{
    public class ChartOfAccountsService : IChartOfAccountsService
    {
        private readonly IChartOfAccountsRepository _repo;
        private readonly ILogger<ChartOfAccountsService> _logger;

        public ChartOfAccountsService(IChartOfAccountsRepository repo, ILogger<ChartOfAccountsService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<DataTable> GetGridDataAsync(string u, ChartOfAccountsRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, ChartOfAccountsAction.GRIDDATA, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> GetSingleRecordAsync(string u, ChartOfAccountsRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, ChartOfAccountsAction.SINGELRECORD, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> InsertRecordAsync(string u, ChartOfAccountsRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, ChartOfAccountsAction.INSERT, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> UpdateRecordAsync(string u, ChartOfAccountsRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, ChartOfAccountsAction.UPDATE, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> DeleteRecordAsync(string u, ChartOfAccountsRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, ChartOfAccountsAction.DELETE, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> GetParentAccountsAsync(string u, ChartOfAccountsRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, ChartOfAccountsAction.GET_PARENT_ACCOUNTS, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> GetNextAccountCodeAsync(string u, ChartOfAccountsRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, ChartOfAccountsAction.GET_NEXT_ACCOUNT_CODE, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> GetTreeAsync(string u, ChartOfAccountsRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, ChartOfAccountsAction.GET_TREE, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }
    }
}