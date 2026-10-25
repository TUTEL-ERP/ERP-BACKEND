using server.Dto;
using System.Data;

namespace server.Interfaces.Services
{
    public interface IDepartmentalConsumptionService
    {
        Task<DataTable> GetGridDataAsync(string usrname, DepartmentalConsumptionDto request);
        Task<DataSet> GetMasterDetailAsync(string usrname, DepartmentalConsumptionDto request);
        Task<DataTable> InsertRecordAsync(string usrname, DepartmentalConsumptionDto request);
        Task<DataTable> UpdateRecordAsync(string usrname, DepartmentalConsumptionDto request);
        Task<DataTable> DeleteRecordAsync(string usrname, DepartmentalConsumptionDto request);
        Task<DataTable> GetDepartmentsAsync(string usrname, DepartmentalConsumptionDto request);
        Task<DataTable> GetItemsAsync(string usrname, DepartmentalConsumptionDto request);
        Task<DataTable> GetItemBatchesAsync(string usrname, DepartmentalConsumptionDto request);
        Task<string> GetNextNumberAsync(string usrname, DepartmentalConsumptionDto request);
        Task<DataTable> ApproveAsync(string usrname, DepartmentalConsumptionDto request);
        Task<DataTable> RejectAsync(string usrname, DepartmentalConsumptionDto request);
    }
}
