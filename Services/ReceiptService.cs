using server.Dto;
using server.Enums;
using server.Interfaces.Repository;
using server.Interfaces.Services;
using System.Data;

namespace server.Services
{
    public class ReceiptService : IReceiptService
    {
        private readonly IReceiptRepository _repo;
        private readonly ILogger<ReceiptService> _logger;

        public ReceiptService(IReceiptRepository repo, ILogger<ReceiptService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<DataTable> GetGridDataAsync(string u, ReceiptRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, ReceiptAction.GRIDDATA, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataSet> GetMasterDetailAsync(string u, ReceiptRequestDto r)
            => await _repo.ExecuteProcedureAsync(u, ReceiptAction.SINGELRECORD, r);

        public async Task<DataTable> InsertRecordAsync(string u, ReceiptRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, ReceiptAction.INSERT, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> UpdateRecordAsync(string u, ReceiptRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, ReceiptAction.UPDATE, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> DeleteRecordAsync(string u, ReceiptRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, ReceiptAction.DELETE, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> GetAllCustomersAsync(string u, ReceiptRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, ReceiptAction.GET_CUSTOMERS, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> GetOpenSalesByCustomerAsync(string u, ReceiptRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, ReceiptAction.GET_OPEN_SALES_BY_CUSTOMER, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<string> GetNextReceiptNumberAsync(string u, ReceiptRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, ReceiptAction.GET_NEXT_RECEIPT_NUMBER, r);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][0]?.ToString() ?? "";
            return "";
        }
    }
}