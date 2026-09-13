using server.Dto;
using System.Data;

namespace server.Interfaces.Services
{
    public interface IAreaService
    {
        Task<DataTable> GetGridDataAsync(string usrname, AreaRequestDto request);
        Task<DataTable> InsertRecordAsync(string usrname, AreaRequestDto request);
        Task<DataTable> UpdateRecordAsync(string usrname, AreaRequestDto request);
        Task<DataTable> DeleteRecordAsync(string usrname, AreaRequestDto request);
        Task<DataTable> SelectRecordAsync(string usrname, AreaRequestDto request);
    }
}