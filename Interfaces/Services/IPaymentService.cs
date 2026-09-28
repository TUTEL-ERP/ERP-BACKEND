using server.Dto;
using System.Data;

namespace server.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<DataTable> GetGridDataAsync(string u, PaymentRequestDto r);
        Task<DataSet> GetMasterDetailAsync(string u, PaymentRequestDto r);
        Task<DataTable> InsertRecordAsync(string u, PaymentRequestDto r);
        Task<DataTable> UpdateRecordAsync(string u, PaymentRequestDto r);
        Task<DataTable> DeleteRecordAsync(string u, PaymentRequestDto r);
        Task<DataTable> GetAllSuppliersAsync(string u, PaymentRequestDto r);
        Task<DataTable> GetOpenPOsBySupplierAsync(string u, PaymentRequestDto r);
        Task<string> GetNextPaymentNumberAsync(string u, PaymentRequestDto r);
    }
}