using server.Dto;
using server.Enums;
using System.Data;

namespace server.Interfaces.Repository
{
    public interface IReceiptAdjustmentRepository
    {
        Task<DataSet> ExecuteProcedureAsync(string usrname, ReceiptAdjustmentAction action, ReceiptAdjustmentRequestDto request);
    }
}