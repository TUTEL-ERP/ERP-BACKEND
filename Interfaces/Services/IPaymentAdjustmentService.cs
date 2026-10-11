using server.Dto;
using System.Data;

namespace server.Interfaces.Services
{
    public interface IPaymentAdjustmentService
    {
        Task<DataTable> GetGridDataAsync(string u, PaymentAdjustmentRequestDto r);
        Task<DataSet> GetMasterDetailAsync(string u, PaymentAdjustmentRequestDto r);
        Task<DataTable> InsertRecordAsync(string u, PaymentAdjustmentRequestDto r);
        Task<DataTable> UpdateRecordAsync(string u, PaymentAdjustmentRequestDto r);
        Task<DataTable> DeleteRecordAsync(string u, PaymentAdjustmentRequestDto r);
        Task<DataTable> GetAllSuppliersAsync(string u, PaymentAdjustmentRequestDto r);
        Task<DataTable> GetPaymentsBySupplierAsync(string u, PaymentAdjustmentRequestDto r);
        Task<string> GetNextAdjustmentNumberAsync(string u, PaymentAdjustmentRequestDto r);
        Task<DataTable> ApproveAsync(string u, PaymentAdjustmentRequestDto r);
        Task<DataTable> RejectAsync(string u, PaymentAdjustmentRequestDto r);
    }
}