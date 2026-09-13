using server.Dto;
using System.Data;

namespace server.Interfaces.Services
{
    public interface ISupplierService
    {
        Task<DataTable> GetGridDataAsync(string usrname, SupplierRequestDto request);
        Task<DataTable> InsertRecordAsync(string usrname, SupplierRequestDto request);
        Task<DataTable> UpdateRecordAsync(string usrname, SupplierRequestDto request);
        Task<DataTable> DeleteRecordAsync(string usrname, SupplierRequestDto request);
        Task<DataTable> SelectRecordAsync(string usrname, SupplierRequestDto request);
    }
}