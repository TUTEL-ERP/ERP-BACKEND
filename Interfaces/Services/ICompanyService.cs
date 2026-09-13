using server.Dto;
using System.Data;

namespace server.Interfaces.Services
{
    public interface ICompanyService
    {
        Task<DataTable> GetGridDataAsync(string usrname, CompanyRequestDto request);
        Task<DataTable> InsertRecordAsync(string usrname, CompanyRequestDto request);
        Task<DataTable> UpdateRecordAsync(string usrname, CompanyRequestDto request);
        Task<DataTable> DeleteRecordAsync(string usrname, CompanyRequestDto request);
        Task<DataTable> SelectRecordAsync(string usrname, CompanyRequestDto request);
    }
}