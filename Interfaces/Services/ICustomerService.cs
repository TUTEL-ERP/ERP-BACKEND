using server.Dto;
using System.Data;

namespace server.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<DataTable> GetGridDataAsync(string usrname, CustomerRequestDto request);
        Task<DataTable> InsertRecordAsync(string usrname, CustomerRequestDto request);
        Task<DataTable> UpdateRecordAsync(string usrname, CustomerRequestDto request);
        Task<DataTable> DeleteRecordAsync(string usrname, CustomerRequestDto request);
        Task<DataTable> SelectRecordAsync(string usrname, CustomerRequestDto request);
    }
}