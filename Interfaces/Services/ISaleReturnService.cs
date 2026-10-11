using server.Dto;
using System.Data;

namespace server.Interfaces.Services
{
    public interface ISaleReturnService
    {
        Task<DataTable> GetGridDataAsync(string u, SaleReturnRequestDto r);
        Task<DataSet> GetMasterDetailAsync(string u, SaleReturnRequestDto r);
        Task<DataTable> InsertRecordAsync(string u, SaleReturnRequestDto r);
        Task<DataTable> UpdateRecordAsync(string u, SaleReturnRequestDto r);
        Task<DataTable> DeleteRecordAsync(string u, SaleReturnRequestDto r);
        Task<DataTable> GetAllItemsAsync(string u, SaleReturnRequestDto r);
        Task<DataTable> GetAllCustomersAsync(string u, SaleReturnRequestDto r);
        Task<DataTable> GetOpenSalesByCustomerAsync(string u, SaleReturnRequestDto r);
        Task<string> GetNextSaleReturnNumberAsync(string u, SaleReturnRequestDto r);
        Task<DataTable> ApproveAsync(string u, SaleReturnRequestDto r);
        Task<DataTable> RejectAsync(string u, SaleReturnRequestDto r);
    }
}