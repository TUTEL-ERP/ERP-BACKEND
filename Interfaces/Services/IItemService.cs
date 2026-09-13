using server.Dto;
using System.Data;

namespace server.Interfaces.Services
{
    public interface IItemService
    {
        Task<DataTable> GetGridDataAsync(string usrname, ItemRequestDto request);
        Task<DataTable> InsertRecordAsync(string usrname, ItemRequestDto request);
        Task<DataTable> UpdateRecordAsync(string usrname, ItemRequestDto request);
        Task<DataTable> DeleteRecordAsync(string usrname, ItemRequestDto request);
        Task<DataTable> SelectRecordAsync(string usrname, ItemRequestDto request);
    }
}