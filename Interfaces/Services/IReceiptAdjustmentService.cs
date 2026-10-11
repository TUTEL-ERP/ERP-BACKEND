using server.Dto;
using System.Data;

namespace server.Interfaces.Services
{
    public interface IReceiptAdjustmentService
    {
        Task<DataTable> GetGridDataAsync(string u, ReceiptAdjustmentRequestDto r);
        Task<DataSet> GetMasterDetailAsync(string u, ReceiptAdjustmentRequestDto r);
        Task<DataTable> InsertRecordAsync(string u, ReceiptAdjustmentRequestDto r);
        Task<DataTable> UpdateRecordAsync(string u, ReceiptAdjustmentRequestDto r);
        Task<DataTable> DeleteRecordAsync(string u, ReceiptAdjustmentRequestDto r);
        Task<DataTable> GetAllCustomersAsync(string u, ReceiptAdjustmentRequestDto r);
        Task<DataTable> GetReceiptsByCustomerAsync(string u, ReceiptAdjustmentRequestDto r);
        Task<string> GetNextAdjustmentNumberAsync(string u, ReceiptAdjustmentRequestDto r);
        Task<DataTable> ApproveAsync(string u, ReceiptAdjustmentRequestDto r);
        Task<DataTable> RejectAsync(string u, ReceiptAdjustmentRequestDto r);
    }
}