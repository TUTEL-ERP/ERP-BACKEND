using server.Dto;
using server.Enums;
using System.Data;

namespace server.Interfaces.Repository
{
    public interface IPaymentAdjustmentRepository
    {
        Task<DataSet> ExecuteProcedureAsync(string usrname, PaymentAdjustmentAction action, PaymentAdjustmentRequestDto request);
    }
}