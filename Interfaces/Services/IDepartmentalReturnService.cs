using server.Dto;
using server.Enums;
using System.Data;

namespace server.Interfaces.Services
{
    public interface IDepartmentalReturnService
    {
        Task<DataTable> GetGridDataAsync(string user, DepartmentalReturnDto request);
        Task<DataSet> GetMasterDetailAsync(string user, DepartmentalReturnDto request);
        Task<DataTable> InsertRecordAsync(string user, DepartmentalReturnDto request);
        Task<DataTable> UpdateRecordAsync(string user, DepartmentalReturnDto request);
        Task<DataTable> DeleteRecordAsync(string user, DepartmentalReturnDto request);
        Task<DataTable> ApproveAsync(string user, DepartmentalReturnDto request);
        Task<DataTable> RejectAsync(string user, DepartmentalReturnDto request);
        Task<DataTable> GetLovAsync(string user, DepartmentalReturnAction action, DepartmentalReturnDto request);
        Task<string> GetNextNumberAsync(string user, DepartmentalReturnDto request);
    }
}
