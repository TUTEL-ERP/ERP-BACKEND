using server.Dto;
using System.Data;

namespace server.Interfaces.Services
{
    public interface IGRNService
    {
        Task<DataTable> GetGridDataAsync(string u, GRNRequestDto r);
        Task<DataSet> GetMasterDetailAsync(string u, GRNRequestDto r);
        Task<DataTable> InsertRecordAsync(string u, GRNRequestDto r);
        Task<DataTable> UpdateRecordAsync(string u, GRNRequestDto r);
        Task<DataTable> DeleteRecordAsync(string u, GRNRequestDto r);
Task<DataTable> GetAllApprovedPOsAsync(string u, GRNRequestDto r);
        Task<DataTable> GetPOItemsAsync(string u, GRNRequestDto r); 
        Task<DataTable> GetAllItemsAsync(string u, GRNRequestDto r);
        Task<string> GetNextGRNNumberAsync(string u, GRNRequestDto r);
        Task<DataTable> ApproveAsync(string u, GRNRequestDto r);
        Task<DataTable> RejectAsync(string u, GRNRequestDto r);
    }
}