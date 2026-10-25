using server.Dto;
using server.Enums;
using server.Interfaces.Repository;
using server.Interfaces.Services;
using System.Data;

namespace server.Services
{
    public class DepartmentalConsumptionService : IDepartmentalConsumptionService
    {
        private readonly IDepartmentalConsumptionRepository _repository;
        private readonly ILogger<DepartmentalConsumptionService> _logger;

        public DepartmentalConsumptionService(
            IDepartmentalConsumptionRepository repository,
            ILogger<DepartmentalConsumptionService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<DataTable> GetGridDataAsync(string usrname, DepartmentalConsumptionDto request) =>
            FirstTable(await _repository.ExecuteProcedureAsync(
                usrname, DepartmentalConsumptionAction.GRIDDATA, request));

        public Task<DataSet> GetMasterDetailAsync(string usrname, DepartmentalConsumptionDto request) =>
            _repository.ExecuteProcedureAsync(usrname, DepartmentalConsumptionAction.SINGELRECORD, request);

        public async Task<DataTable> InsertRecordAsync(string usrname, DepartmentalConsumptionDto request) =>
            FirstTable(await _repository.ExecuteProcedureAsync(
                usrname, DepartmentalConsumptionAction.INSERT, request));

        public async Task<DataTable> UpdateRecordAsync(string usrname, DepartmentalConsumptionDto request) =>
            FirstTable(await _repository.ExecuteProcedureAsync(
                usrname, DepartmentalConsumptionAction.UPDATE, request));

        public async Task<DataTable> DeleteRecordAsync(string usrname, DepartmentalConsumptionDto request) =>
            FirstTable(await _repository.ExecuteProcedureAsync(
                usrname, DepartmentalConsumptionAction.DELETE, request));

        public async Task<DataTable> GetDepartmentsAsync(string usrname, DepartmentalConsumptionDto request) =>
            FirstTable(await _repository.ExecuteProcedureAsync(
                usrname, DepartmentalConsumptionAction.GET_DEPARTMENTS, request));

        public async Task<DataTable> GetItemsAsync(string usrname, DepartmentalConsumptionDto request) =>
            FirstTable(await _repository.ExecuteProcedureAsync(
                usrname, DepartmentalConsumptionAction.GET_ITEMS, request));

        public async Task<DataTable> GetItemBatchesAsync(string usrname, DepartmentalConsumptionDto request) =>
            FirstTable(await _repository.ExecuteProcedureAsync(
                usrname, DepartmentalConsumptionAction.GET_ITEM_BATCHES, request));

        public async Task<string> GetNextNumberAsync(string usrname, DepartmentalConsumptionDto request)
        {
            var result = FirstTable(await _repository.ExecuteProcedureAsync(
                usrname, DepartmentalConsumptionAction.GET_NEXT_CONSUMPTION_NO, request));
            return result.Rows.Count > 0 ? result.Rows[0][0]?.ToString() ?? string.Empty : string.Empty;
        }

        public async Task<DataTable> ApproveAsync(string usrname, DepartmentalConsumptionDto request) =>
            FirstTable(await _repository.ExecuteProcedureAsync(
                usrname, DepartmentalConsumptionAction.APPROVE, request));

        public async Task<DataTable> RejectAsync(string usrname, DepartmentalConsumptionDto request) =>
            FirstTable(await _repository.ExecuteProcedureAsync(
                usrname, DepartmentalConsumptionAction.REJECT, request));

        private DataTable FirstTable(DataSet dataSet)
        {
            if (dataSet.Tables.Count == 0)
            {
                _logger.LogError("Departmental Consumption procedure returned no result table.");
                throw new InvalidOperationException("Stored procedure returned no result table.");
            }

            return dataSet.Tables[0];
        }
    }
}
