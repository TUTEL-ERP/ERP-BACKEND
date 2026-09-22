// server/Services/PurchaseRequisitionService.cs
using server.Dto;
using server.Enums;
using server.Interfaces.Repository;
using server.Interfaces.Services;
using System.Data;

namespace server.Services
{
    public class PurchaseRequisitionService : IPurchaseRequisitionService
    {
        private readonly IPurchaseRequisitionRepository _repo;
        private readonly ILogger<PurchaseRequisitionService> _logger;

        public PurchaseRequisitionService(IPurchaseRequisitionRepository repo, ILogger<PurchaseRequisitionService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<DataTable> GetGridDataAsync(string u, PurchaseRequisitionRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, PurchaseRequisitionAction.GRIDDATA, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataSet> GetMasterDetailAsync(string u, PurchaseRequisitionRequestDto r)
            => await _repo.ExecuteProcedureAsync(u, PurchaseRequisitionAction.SINGELRECORD, r);

        public async Task<DataTable> InsertRecordAsync(string u, PurchaseRequisitionRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, PurchaseRequisitionAction.INSERT, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> UpdateRecordAsync(string u, PurchaseRequisitionRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, PurchaseRequisitionAction.UPDATE, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> DeleteRecordAsync(string u, PurchaseRequisitionRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, PurchaseRequisitionAction.DELETE, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> GetAllItemsAsync(string u, PurchaseRequisitionRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, PurchaseRequisitionAction.GI, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }
    }
}