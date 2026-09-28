using server.Dto;
using server.Enums;
using System.Data;

namespace server.Interfaces.Repository
{
    public interface IPaymentRepository
    {
        Task<DataSet> ExecuteProcedureAsync(string usrname, PaymentAction action, PaymentRequestDto request);
    }
}