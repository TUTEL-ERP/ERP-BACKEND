// server/Interfaces/Services/IPurchaseRequisitionService.cs
using server.Dto;
using System.Data;

namespace server.Interfaces.Services
{
    public interface IPurchaseRequisitionService
    {
        Task<DataTable> GetGridDataAsync(string usrname, PurchaseRequisitionRequestDto request);
        Task<DataSet> GetMasterDetailAsync(string usrname, PurchaseRequisitionRequestDto request);
        Task<DataTable> InsertRecordAsync(string usrname, PurchaseRequisitionRequestDto request);
        Task<DataTable> UpdateRecordAsync(string usrname, PurchaseRequisitionRequestDto request);
        Task<DataTable> DeleteRecordAsync(string usrname, PurchaseRequisitionRequestDto request);
    }
}