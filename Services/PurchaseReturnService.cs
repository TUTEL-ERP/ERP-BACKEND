using server.Dto;
using server.Enums;
using server.Interfaces.Repository;
using server.Interfaces.Services;
using System.Data;

namespace server.Services
{
    public class PurchaseReturnService : IPurchaseReturnService
    {
        private readonly IPurchaseReturnRepository _repo;
        private readonly ILogger<PurchaseReturnService> _logger;

        public PurchaseReturnService(IPurchaseReturnRepository repo, ILogger<PurchaseReturnService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<DataTable> GetGridDataAsync(string u, PurchaseReturnRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, PurchaseReturnAction.GRIDDATA, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataSet> GetMasterDetailAsync(string u, PurchaseReturnRequestDto r)
            => await _repo.ExecuteProcedureAsync(u, PurchaseReturnAction.SINGELRECORD, r);

        public async Task<DataTable> InsertRecordAsync(string u, PurchaseReturnRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, PurchaseReturnAction.INSERT, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> UpdateRecordAsync(string u, PurchaseReturnRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, PurchaseReturnAction.UPDATE, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> DeleteRecordAsync(string u, PurchaseReturnRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, PurchaseReturnAction.DELETE, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> GetAllItemsAsync(string u, PurchaseReturnRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, PurchaseReturnAction.LOV_RETURN_ITEM, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }
        public async Task<DataTable> GetAllSuppliersAsync(string u, PurchaseReturnRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, PurchaseReturnAction.GET_SUPPLIERS, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }
        public async Task<string> GetNextReturnNumberAsync(string u, PurchaseReturnRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, PurchaseReturnAction.GET_NEXT_RETURN_NUMBER, r);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][0]?.ToString() ?? "";
            return "";
        }
    }
}