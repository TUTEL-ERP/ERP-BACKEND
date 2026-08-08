using server.Dto;
using System.Data;

namespace server.Interfaces.Services
{
    public interface IProvinceService
    {
        Task<DataTable> GetGridDataAsync(string usrname, ProvinceRequestDto request);
        Task<DataTable> InsertRecordAsync(string usrname, ProvinceRequestDto request);
        Task<DataTable> UpdateRecordAsync(string usrname, ProvinceRequestDto request);
        Task<DataTable> DeleteRecordAsync(string usrname, ProvinceRequestDto request);
        Task<DataTable> SelectRecordAsync(string usrname, ProvinceRequestDto request);
    }
}