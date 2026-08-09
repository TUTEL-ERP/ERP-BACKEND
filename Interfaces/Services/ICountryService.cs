using server.Dto;
using System.Data;

namespace server.Interfaces.Services
{
    public interface ICountryService
    {
        Task<DataTable> GetGridDataAsync(string usrname, CountryRequestDto request);
        Task<DataTable> InsertRecordAsync(string usrname, CountryRequestDto request);
        Task<DataTable> UpdateRecordAsync(string usrname, CountryRequestDto request);
        Task<DataTable> DeleteRecordAsync(string usrname, CountryRequestDto request);
        Task<DataTable> SelectRecordAsync(string usrname, CountryRequestDto request);
    }
}