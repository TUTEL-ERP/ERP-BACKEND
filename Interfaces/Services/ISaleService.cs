using server.Dto;
using System.Data;

namespace server.Interfaces.Services
{
    public interface ISaleService
    {
        Task<DataTable> GetGridDataAsync(string u, SaleRequestDto r);
        Task<DataSet> GetMasterDetailAsync(string u, SaleRequestDto r);
        Task<DataTable> InsertRecordAsync(string u, SaleRequestDto r);
        Task<DataTable> UpdateRecordAsync(string u, SaleRequestDto r);
        Task<DataTable> DeleteRecordAsync(string u, SaleRequestDto r);
        Task<DataTable> GetAllItemsAsync(string u, SaleRequestDto r);
        Task<DataTable> GetAllCustomersAsync(string u, SaleRequestDto r);
        Task<string> GetNextSaleNumberAsync(string u, SaleRequestDto r);
    }
}