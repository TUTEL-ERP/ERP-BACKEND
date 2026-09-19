using server.Dto;
using server.Enums;
using System.Data;

namespace server.Interfaces.Repository
{
    public interface IPurchaseRequisitionRepository
    {
        Task<DataSet> ExecuteProcedureAsync(string usrname, PurchaseRequisitionAction action, PurchaseRequisitionRequestDto request);
    }
}