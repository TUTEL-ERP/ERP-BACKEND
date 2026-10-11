using server.Dto;
using server.Enums;
using server.Interfaces.Repository;
using server.Interfaces.Services;
using System.Data;

namespace server.Services
{
    public class SaleReturnService : ISaleReturnService
    {
        private readonly ISaleReturnRepository _repo;
        private readonly ILogger<SaleReturnService> _logger;

        public SaleReturnService(ISaleReturnRepository repo, ILogger<SaleReturnService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<DataTable> GetGridDataAsync(string u, SaleReturnRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, SaleReturnAction.GRIDDATA, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataSet> GetMasterDetailAsync(string u, SaleReturnRequestDto r)
            => await _repo.ExecuteProcedureAsync(u, SaleReturnAction.SINGELRECORD, r);

        public async Task<DataTable> InsertRecordAsync(string u, SaleReturnRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, SaleReturnAction.INSERT, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> UpdateRecordAsync(string u, SaleReturnRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, SaleReturnAction.UPDATE, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> DeleteRecordAsync(string u, SaleReturnRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, SaleReturnAction.DELETE, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> GetAllItemsAsync(string u, SaleReturnRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, SaleReturnAction.GET_ITEMS, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> GetAllCustomersAsync(string u, SaleReturnRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, SaleReturnAction.GET_CUSTOMERS, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> GetOpenSalesByCustomerAsync(string u, SaleReturnRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, SaleReturnAction.GET_OPEN_SALES_BY_CUSTOMER, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<string> GetNextSaleReturnNumberAsync(string u, SaleReturnRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, SaleReturnAction.GET_NEXT_SALE_RETURN_NUMBER, r);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][0]?.ToString() ?? "";
            return "";
        }

        public async Task<DataTable> ApproveAsync(string u, SaleReturnRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, SaleReturnAction.APPROVE, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> RejectAsync(string u, SaleReturnRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, SaleReturnAction.REJECT, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }
    }
}