using server.Dto;
using server.Enums;
using server.Interfaces.Repository;
using server.Interfaces.Services;
using System.Data;

namespace server.Services
{
    public class GRNService : IGRNService
    {
        private readonly IGRNRepository _repo;
        public GRNService(IGRNRepository repo) { _repo = repo; }

        public async Task<DataTable> GetGridDataAsync(string u, GRNRequestDto r) => (await _repo.ExecuteProcedureAsync(u, GRNAction.GRIDDATA, r)).Tables[0];
        public async Task<DataSet> GetMasterDetailAsync(string u, GRNRequestDto r) => await _repo.ExecuteProcedureAsync(u, GRNAction.SINGELRECORD, r);
        public async Task<DataTable> InsertRecordAsync(string u, GRNRequestDto r) => (await _repo.ExecuteProcedureAsync(u, GRNAction.INSERT, r)).Tables[0];
        public async Task<DataTable> UpdateRecordAsync(string u, GRNRequestDto r) => (await _repo.ExecuteProcedureAsync(u, GRNAction.UPDATE, r)).Tables[0];
        public async Task<DataTable> DeleteRecordAsync(string u, GRNRequestDto r) => (await _repo.ExecuteProcedureAsync(u, GRNAction.DELETE, r)).Tables[0];
public async Task<DataTable> GetAllApprovedPOsAsync(string u, GRNRequestDto r) 
    => (await _repo.ExecuteProcedureAsync(u, GRNAction.GET_APPROVED_POS, r)).Tables[0];
         public async Task<DataTable> GetPOItemsAsync(string u, GRNRequestDto r)
            => (await _repo.ExecuteProcedureAsync(u, GRNAction.GET_PO_ITEMS, r)).Tables[0];

            public async Task<DataTable> GetAllItemsAsync(string u, GRNRequestDto r) => (await _repo.ExecuteProcedureAsync(u, GRNAction.GET_ITEMS, r)).Tables[0];
        public async Task<DataTable> ApproveAsync(string u, GRNRequestDto r) => (await _repo.ExecuteProcedureAsync(u, GRNAction.APPROVE, r)).Tables[0];
        public async Task<DataTable> RejectAsync(string u, GRNRequestDto r) => (await _repo.ExecuteProcedureAsync(u, GRNAction.REJECT, r)).Tables[0];
        
        public async Task<string> GetNextGRNNumberAsync(string u, GRNRequestDto r)
        {
            var dt = (await _repo.ExecuteProcedureAsync(u, GRNAction.GET_NEXT_GRN_NUMBER, r)).Tables[0];
            return dt.Rows.Count > 0 ? dt.Rows[0][0].ToString() : "";
        }
    }
}