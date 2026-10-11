using server.Dto;
using server.Enums;
using server.Interfaces.Repository;
using server.Interfaces.Services;
using System.Data;

namespace server.Services
{
    public class PaymentAdjustmentService : IPaymentAdjustmentService
    {
        private readonly IPaymentAdjustmentRepository _repo;
        private readonly ILogger<PaymentAdjustmentService> _logger;

        public PaymentAdjustmentService(IPaymentAdjustmentRepository repo, ILogger<PaymentAdjustmentService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<DataTable> GetGridDataAsync(string u, PaymentAdjustmentRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, PaymentAdjustmentAction.GRIDDATA, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataSet> GetMasterDetailAsync(string u, PaymentAdjustmentRequestDto r)
            => await _repo.ExecuteProcedureAsync(u, PaymentAdjustmentAction.SINGELRECORD, r);

        public async Task<DataTable> InsertRecordAsync(string u, PaymentAdjustmentRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, PaymentAdjustmentAction.INSERT, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> UpdateRecordAsync(string u, PaymentAdjustmentRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, PaymentAdjustmentAction.UPDATE, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> DeleteRecordAsync(string u, PaymentAdjustmentRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, PaymentAdjustmentAction.DELETE, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> GetAllSuppliersAsync(string u, PaymentAdjustmentRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, PaymentAdjustmentAction.GET_SUPPLIERS, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> GetPaymentsBySupplierAsync(string u, PaymentAdjustmentRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, PaymentAdjustmentAction.GET_PAYMENTS_BY_SUPPLIER, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<string> GetNextAdjustmentNumberAsync(string u, PaymentAdjustmentRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, PaymentAdjustmentAction.GET_NEXT_ADJUSTMENT_NUMBER, r);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][0]?.ToString() ?? "";
            return "";
        }

        public async Task<DataTable> ApproveAsync(string u, PaymentAdjustmentRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, PaymentAdjustmentAction.APPROVE, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> RejectAsync(string u, PaymentAdjustmentRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, PaymentAdjustmentAction.REJECT, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }
    }
}