using server.Dto;
using server.Enums;
using System.Data;

namespace server.Interfaces.Repository
{
    public interface IPurchaseReturnRepository
    {
        Task<DataSet> ExecuteProcedureAsync(string usrname, PurchaseReturnAction action, PurchaseReturnRequestDto request);
    }
}