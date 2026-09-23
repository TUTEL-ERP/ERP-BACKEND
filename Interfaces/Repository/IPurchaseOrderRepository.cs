using server.Dto;
using server.Enums;
using System.Data;

namespace server.Interfaces.Repository
{
    public interface IPurchaseOrderRepository
    {
        Task<DataSet> ExecuteProcedureAsync(string usrname, PurchaseOrderAction action, PurchaseOrderRequestDto request);
    }
}