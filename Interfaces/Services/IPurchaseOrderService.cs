using server.Dto;
using System.Data;

namespace server.Interfaces.Services
{
    public interface IPurchaseOrderService
    {
        Task<DataTable> GetGridDataAsync(string u, PurchaseOrderRequestDto r);
        Task<DataSet> GetMasterDetailAsync(string u, PurchaseOrderRequestDto r);
        Task<DataTable> InsertRecordAsync(string u, PurchaseOrderRequestDto r);
        Task<DataTable> UpdateRecordAsync(string u, PurchaseOrderRequestDto r);
        Task<DataTable> DeleteRecordAsync(string u, PurchaseOrderRequestDto r);
        Task<DataTable> GetAllItemsAsync(string u, PurchaseOrderRequestDto r);
        Task<string> GetNextPONumberAsync(string u, PurchaseOrderRequestDto r);
    }
}