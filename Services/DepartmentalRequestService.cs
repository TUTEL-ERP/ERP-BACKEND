using server.Dto;
using server.Enums;
using server.Interfaces.Repository;
using server.Interfaces.Services;
using System.Data;

namespace server.Services
{
    public class DepartmentalRequestService : IDepartmentalRequestService
    {
        private readonly IDepartmentalRequestRepository _repo;
        public DepartmentalRequestService(IDepartmentalRequestRepository repo) { _repo = repo; }

        public async Task<DataTable> GetGridDataAsync(string u, DepartmentalRequestDto r) => (await _repo.ExecuteProcedureAsync(u, DepartmentalRequestAction.GRIDDATA, r)).Tables[0];
        public async Task<DataSet> GetMasterDetailAsync(string u, DepartmentalRequestDto r) => await _repo.ExecuteProcedureAsync(u, DepartmentalRequestAction.SINGELRECORD, r);
        public async Task<DataTable> InsertRecordAsync(string u, DepartmentalRequestDto r) => (await _repo.ExecuteProcedureAsync(u, DepartmentalRequestAction.INSERT, r)).Tables[0];
        public async Task<DataTable> UpdateRecordAsync(string u, DepartmentalRequestDto r) => (await _repo.ExecuteProcedureAsync(u, DepartmentalRequestAction.UPDATE, r)).Tables[0];
        public async Task<DataTable> DeleteRecordAsync(string u, DepartmentalRequestDto r) => (await _repo.ExecuteProcedureAsync(u, DepartmentalRequestAction.DELETE, r)).Tables[0];
        public async Task<DataTable> GetDepartmentsAsync(string u, DepartmentalRequestDto r) => (await _repo.ExecuteProcedureAsync(u, DepartmentalRequestAction.GET_DEPARTMENTS, r)).Tables[0];
        public async Task<DataTable> GetItemsAsync(string u, DepartmentalRequestDto r) => (await _repo.ExecuteProcedureAsync(u, DepartmentalRequestAction.GET_ITEMS, r)).Tables[0];
        public async Task<DataTable> ApproveAsync(string u, DepartmentalRequestDto r) => (await _repo.ExecuteProcedureAsync(u, DepartmentalRequestAction.APPROVE, r)).Tables[0];
        public async Task<DataTable> RejectAsync(string u, DepartmentalRequestDto r) => (await _repo.ExecuteProcedureAsync(u, DepartmentalRequestAction.REJECT, r)).Tables[0];
        
        public async Task<string> GetNextRequestNoAsync(string u, DepartmentalRequestDto r)
        {
            var dt = (await _repo.ExecuteProcedureAsync(u, DepartmentalRequestAction.GET_NEXT_REQUEST_NO, r)).Tables[0];
            return dt.Rows.Count > 0 ? dt.Rows[0][0].ToString() : "";
        }
    }
}