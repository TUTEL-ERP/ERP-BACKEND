using server.Dto;
using System.Data;

namespace server.Interfaces.Services
{
    public interface IPurchaseReturnService
    {
        Task<DataTable> GetGridDataAsync(string u, PurchaseReturnRequestDto r);
        Task<DataSet> GetMasterDetailAsync(string u, PurchaseReturnRequestDto r);
        Task<DataTable> InsertRecordAsync(string u, PurchaseReturnRequestDto r);
        Task<DataTable> UpdateRecordAsync(string u, PurchaseReturnRequestDto r);
        Task<DataTable> DeleteRecordAsync(string u, PurchaseReturnRequestDto r);
        Task<DataTable> GetAllItemsAsync(string u, PurchaseReturnRequestDto r);
        Task<DataTable> GetAllSuppliersAsync(string u, PurchaseReturnRequestDto r);
        Task<string> GetNextReturnNumberAsync(string u, PurchaseReturnRequestDto r);
    }
}