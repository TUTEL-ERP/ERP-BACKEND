using server.Dto;
using server.Enums;
using server.Interfaces.Repository;
using server.Interfaces.Services;
using System.Data;

namespace server.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IPurchaseOrderRepository _repo;
        private readonly ILogger<PurchaseOrderService> _logger;

        public PurchaseOrderService(IPurchaseOrderRepository repo, ILogger<PurchaseOrderService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<DataTable> GetGridDataAsync(string u, PurchaseOrderRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, PurchaseOrderAction.GRIDDATA, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataSet> GetMasterDetailAsync(string u, PurchaseOrderRequestDto r)
            => await _repo.ExecuteProcedureAsync(u, PurchaseOrderAction.SINGELRECORD, r);

        public async Task<DataTable> InsertRecordAsync(string u, PurchaseOrderRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, PurchaseOrderAction.INSERT, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> UpdateRecordAsync(string u, PurchaseOrderRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, PurchaseOrderAction.UPDATE, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> DeleteRecordAsync(string u, PurchaseOrderRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, PurchaseOrderAction.DELETE, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> GetAllItemsAsync(string u, PurchaseOrderRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, PurchaseOrderAction.LOV_PO_ITEM, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<string> GetNextPONumberAsync(string u, PurchaseOrderRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, PurchaseOrderAction.GET_NEXT_PO_NUMBER, r);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][0]?.ToString() ?? "";
            return "";
        }
    }
}