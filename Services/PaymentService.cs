using server.Dto;
using server.Enums;
using server.Interfaces.Repository;
using server.Interfaces.Services;
using System.Data;

namespace server.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _repo;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(IPaymentRepository repo, ILogger<PaymentService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<DataTable> GetGridDataAsync(string u, PaymentRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, PaymentAction.GRIDDATA, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataSet> GetMasterDetailAsync(string u, PaymentRequestDto r)
            => await _repo.ExecuteProcedureAsync(u, PaymentAction.SINGELRECORD, r);

        public async Task<DataTable> InsertRecordAsync(string u, PaymentRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, PaymentAction.INSERT, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> UpdateRecordAsync(string u, PaymentRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, PaymentAction.UPDATE, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> DeleteRecordAsync(string u, PaymentRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, PaymentAction.DELETE, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> GetAllSuppliersAsync(string u, PaymentRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, PaymentAction.GET_SUPPLIERS, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> GetOpenPOsBySupplierAsync(string u, PaymentRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, PaymentAction.GET_OPEN_POS_BY_SUPPLIER, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<string> GetNextPaymentNumberAsync(string u, PaymentRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, PaymentAction.GET_NEXT_PAYMENT_NUMBER, r);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][0]?.ToString() ?? "";
            return "";
        }
    }
}