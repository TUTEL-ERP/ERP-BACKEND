using server.Dto;
using System.Data;

namespace server.Interfaces.Services
{
    public interface IUserSetupService
    {
        Task<DataTable> GetGridDataAsync(string usrname, UserRequestDto request);
        Task<DataTable> InsertRecordAsync(string usrname, UserRequestDto request);
        Task<DataTable> UpdateRecordAsync(string usrname, UserRequestDto request);
        Task<DataTable> DeleteRecordAsync(string usrname, UserRequestDto request);
        Task<DataTable> SelectRecordAsync(string usrname, UserRequestDto request);
    }
}