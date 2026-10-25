using server.Dto;
using server.Enums;
using server.Interfaces.Repository;
using server.Interfaces.Services;
using System.Data;

namespace server.Services
{
    public class DepartmentalIssueService : IDepartmentalIssueService
    {
        private readonly IDepartmentalIssueRepository _repository;
        private readonly ILogger<DepartmentalIssueService> _logger;

        public DepartmentalIssueService(
            IDepartmentalIssueRepository repository,
            ILogger<DepartmentalIssueService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<DataTable> GetGridDataAsync(string usrname, DepartmentalIssueDto request) =>
            FirstTable(await _repository.ExecuteProcedureAsync(usrname, DepartmentalIssueAction.GRIDDATA, request));

        public Task<DataSet> GetMasterDetailAsync(string usrname, DepartmentalIssueDto request) =>
            _repository.ExecuteProcedureAsync(usrname, DepartmentalIssueAction.SINGELRECORD, request);

        public async Task<DataTable> InsertRecordAsync(string usrname, DepartmentalIssueDto request) =>
            FirstTable(await _repository.ExecuteProcedureAsync(usrname, DepartmentalIssueAction.INSERT, request));

        public async Task<DataTable> UpdateRecordAsync(string usrname, DepartmentalIssueDto request) =>
            FirstTable(await _repository.ExecuteProcedureAsync(usrname, DepartmentalIssueAction.UPDATE, request));

        public async Task<DataTable> DeleteRecordAsync(string usrname, DepartmentalIssueDto request) =>
            FirstTable(await _repository.ExecuteProcedureAsync(usrname, DepartmentalIssueAction.DELETE, request));

        public async Task<DataTable> GetDepartmentsAsync(string usrname, DepartmentalIssueDto request) =>
            FirstTable(await _repository.ExecuteProcedureAsync(usrname, DepartmentalIssueAction.GET_DEPARTMENTS, request));

        public async Task<DataTable> GetItemsAsync(string usrname, DepartmentalIssueDto request) =>
            FirstTable(await _repository.ExecuteProcedureAsync(usrname, DepartmentalIssueAction.GET_ITEMS, request));

        public async Task<string> GetNextNoAsync(string usrname, DepartmentalIssueDto request)
        {
            var result = FirstTable(await _repository.ExecuteProcedureAsync(
                usrname, DepartmentalIssueAction.GET_NEXT_NO, request));
            return result.Rows.Count > 0 ? result.Rows[0][0]?.ToString() ?? string.Empty : string.Empty;
        }

        public async Task<DataTable> GetApprovedDRsAsync(string usrname, DepartmentalIssueDto request) =>
            FirstTable(await _repository.ExecuteProcedureAsync(
                usrname, DepartmentalIssueAction.GET_APPROVED_DRS, request));

        public async Task<DataTable> GetDRItemsAsync(string usrname, DepartmentalIssueDto request) =>
            FirstTable(await _repository.ExecuteProcedureAsync(
                usrname, DepartmentalIssueAction.GET_DR_ITEMS, request));

        public async Task<DataTable> ApproveAsync(string usrname, DepartmentalIssueDto request) =>
            FirstTable(await _repository.ExecuteProcedureAsync(usrname, DepartmentalIssueAction.APPROVE, request));

        public async Task<DataTable> RejectAsync(string usrname, DepartmentalIssueDto request) =>
            FirstTable(await _repository.ExecuteProcedureAsync(usrname, DepartmentalIssueAction.REJECT, request));

        private DataTable FirstTable(DataSet dataSet)
        {
            if (dataSet.Tables.Count == 0)
            {
                _logger.LogError("Stored procedure returned no result table.");
                throw new InvalidOperationException("Stored procedure returned no result table.");
            }

            return dataSet.Tables[0];
        }
    }
}
