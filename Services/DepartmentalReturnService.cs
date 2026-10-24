using server.Dto;
using server.Enums;
using server.Interfaces.Repository;
using server.Interfaces.Services;
using System.Data;

namespace server.Services
{
    public class DepartmentalReturnService : IDepartmentalReturnService
    {
        private readonly IDepartmentalReturnRepository _repository;

        public DepartmentalReturnService(IDepartmentalReturnRepository repository)
        {
            _repository = repository;
        }

        public async Task<DataTable> GetGridDataAsync(string user, DepartmentalReturnDto request) =>
            (await _repository.ExecuteProcedureAsync(user, DepartmentalReturnAction.GRIDDATA, request)).Tables[0];

        public Task<DataSet> GetMasterDetailAsync(string user, DepartmentalReturnDto request) =>
            _repository.ExecuteProcedureAsync(user, DepartmentalReturnAction.SINGELRECORD, request);

        public async Task<DataTable> InsertRecordAsync(string user, DepartmentalReturnDto request) =>
            (await _repository.ExecuteProcedureAsync(user, DepartmentalReturnAction.INSERT, request)).Tables[0];

        public async Task<DataTable> UpdateRecordAsync(string user, DepartmentalReturnDto request) =>
            (await _repository.ExecuteProcedureAsync(user, DepartmentalReturnAction.UPDATE, request)).Tables[0];

        public async Task<DataTable> DeleteRecordAsync(string user, DepartmentalReturnDto request) =>
            (await _repository.ExecuteProcedureAsync(user, DepartmentalReturnAction.DELETE, request)).Tables[0];

        public async Task<DataTable> ApproveAsync(string user, DepartmentalReturnDto request) =>
            (await _repository.ExecuteProcedureAsync(user, DepartmentalReturnAction.APPROVE, request)).Tables[0];

        public async Task<DataTable> RejectAsync(string user, DepartmentalReturnDto request) =>
            (await _repository.ExecuteProcedureAsync(user, DepartmentalReturnAction.REJECT, request)).Tables[0];

        public async Task<DataTable> GetLovAsync(string user, DepartmentalReturnAction action, DepartmentalReturnDto request)
        {
            if (action is not (DepartmentalReturnAction.GET_DEPARTMENTS or DepartmentalReturnAction.GET_ITEMS))
                throw new ArgumentOutOfRangeException(nameof(action), action, "Only lookup actions are supported.");

            return (await _repository.ExecuteProcedureAsync(user, action, request)).Tables[0];
        }

        public async Task<string> GetNextNumberAsync(string user, DepartmentalReturnDto request)
        {
            var dataSet = await _repository.ExecuteProcedureAsync(user, DepartmentalReturnAction.GET_NEXT_NO, request);
            return dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0
                ? dataSet.Tables[0].Rows[0][0]?.ToString() ?? string.Empty
                : string.Empty;
        }
    }
}
