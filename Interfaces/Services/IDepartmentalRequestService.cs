using server.Dto;
using System.Data;

namespace server.Interfaces.Services
{
    public interface IDepartmentalRequestService
    {
        Task<DataTable> GetGridDataAsync(string u, DepartmentalRequestDto r);
        Task<DataSet> GetMasterDetailAsync(string u, DepartmentalRequestDto r);
        Task<DataTable> InsertRecordAsync(string u, DepartmentalRequestDto r);
        Task<DataTable> UpdateRecordAsync(string u, DepartmentalRequestDto r);
        Task<DataTable> DeleteRecordAsync(string u, DepartmentalRequestDto r);
        Task<DataTable> GetDepartmentsAsync(string u, DepartmentalRequestDto r);
        Task<DataTable> GetItemsAsync(string u, DepartmentalRequestDto r);
        Task<string> GetNextRequestNoAsync(string u, DepartmentalRequestDto r);
        Task<DataTable> ApproveAsync(string u, DepartmentalRequestDto r);
        Task<DataTable> RejectAsync(string u, DepartmentalRequestDto r);
    }
}