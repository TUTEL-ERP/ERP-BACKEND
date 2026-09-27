using server.Dto;
using server.Enums;
using server.Interfaces.Repository;
using server.Interfaces.Services;
using System.Data;

namespace server.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _repo;
        private readonly ILogger<SaleService> _logger;

        public SaleService(ISaleRepository repo, ILogger<SaleService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<DataTable> GetGridDataAsync(string u, SaleRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, SaleAction.GRIDDATA, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataSet> GetMasterDetailAsync(string u, SaleRequestDto r)
            => await _repo.ExecuteProcedureAsync(u, SaleAction.SINGELRECORD, r);

        public async Task<DataTable> InsertRecordAsync(string u, SaleRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, SaleAction.INSERT, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> UpdateRecordAsync(string u, SaleRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, SaleAction.UPDATE, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> DeleteRecordAsync(string u, SaleRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, SaleAction.DELETE, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> GetAllItemsAsync(string u, SaleRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, SaleAction.GET_ITEMS, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> GetAllCustomersAsync(string u, SaleRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, SaleAction.GET_CUSTOMERS, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<string> GetNextSaleNumberAsync(string u, SaleRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, SaleAction.GET_NEXT_SALE_NUMBER, r);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][0]?.ToString() ?? "";
            return "";
        }
    }
}