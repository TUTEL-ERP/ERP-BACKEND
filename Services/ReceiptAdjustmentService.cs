using server.Dto;
using server.Enums;
using server.Interfaces.Repository;
using server.Interfaces.Services;
using System.Data;

namespace server.Services
{
    public class ReceiptAdjustmentService : IReceiptAdjustmentService
    {
        private readonly IReceiptAdjustmentRepository _repo;
        private readonly ILogger<ReceiptAdjustmentService> _logger;

        public ReceiptAdjustmentService(IReceiptAdjustmentRepository repo, ILogger<ReceiptAdjustmentService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<DataTable> GetGridDataAsync(string u, ReceiptAdjustmentRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, ReceiptAdjustmentAction.GRIDDATA, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataSet> GetMasterDetailAsync(string u, ReceiptAdjustmentRequestDto r)
            => await _repo.ExecuteProcedureAsync(u, ReceiptAdjustmentAction.SINGELRECORD, r);

        public async Task<DataTable> InsertRecordAsync(string u, ReceiptAdjustmentRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, ReceiptAdjustmentAction.INSERT, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> UpdateRecordAsync(string u, ReceiptAdjustmentRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, ReceiptAdjustmentAction.UPDATE, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> DeleteRecordAsync(string u, ReceiptAdjustmentRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, ReceiptAdjustmentAction.DELETE, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> GetAllCustomersAsync(string u, ReceiptAdjustmentRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, ReceiptAdjustmentAction.GET_CUSTOMERS, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> GetReceiptsByCustomerAsync(string u, ReceiptAdjustmentRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, ReceiptAdjustmentAction.GET_RECEIPTS_BY_CUSTOMER, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<string> GetNextAdjustmentNumberAsync(string u, ReceiptAdjustmentRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, ReceiptAdjustmentAction.GET_NEXT_ADJUSTMENT_NUMBER, r);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][0]?.ToString() ?? "";
            return "";
        }

        public async Task<DataTable> ApproveAsync(string u, ReceiptAdjustmentRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, ReceiptAdjustmentAction.APPROVE, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> RejectAsync(string u, ReceiptAdjustmentRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, ReceiptAdjustmentAction.REJECT, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }
    }
}