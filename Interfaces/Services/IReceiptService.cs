using server.Dto;
using System.Data;

namespace server.Interfaces.Services
{
    public interface IReceiptService
    {
        Task<DataTable> GetGridDataAsync(string u, ReceiptRequestDto r);
        Task<DataSet> GetMasterDetailAsync(string u, ReceiptRequestDto r);
        Task<DataTable> InsertRecordAsync(string u, ReceiptRequestDto r);
        Task<DataTable> UpdateRecordAsync(string u, ReceiptRequestDto r);
        Task<DataTable> DeleteRecordAsync(string u, ReceiptRequestDto r);
        Task<DataTable> GetAllCustomersAsync(string u, ReceiptRequestDto r);
        Task<DataTable> GetOpenSalesByCustomerAsync(string u, ReceiptRequestDto r);
        Task<string> GetNextReceiptNumberAsync(string u, ReceiptRequestDto r);
    }
}