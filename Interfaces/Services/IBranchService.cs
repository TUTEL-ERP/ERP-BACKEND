using server.Dto;
using System.Data;

namespace server.Interfaces.Services
{
    public interface IBranchService
    {
        Task<DataTable> GetGridDataAsync(string usrname, BranchRequestDto request);
        Task<DataTable> GetLovDataAsync(string usrname, BranchRequestDto request);
        Task<DataTable> InsertRecordAsync(string usrname, BranchRequestDto request);
        Task<DataTable> UpdateRecordAsync(string usrname, BranchRequestDto request);
        Task<DataTable> DeleteRecordAsync(string usrname, BranchRequestDto request);
        Task<DataTable> SelectRecordAsync(string usrname, BranchRequestDto request);
    }
}
