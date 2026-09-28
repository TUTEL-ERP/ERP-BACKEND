using server.Dto;
using server.Enums;
using System.Data;

namespace server.Interfaces.Repository
{
    public interface IReceiptRepository
    {
        Task<DataSet> ExecuteProcedureAsync(string usrname, ReceiptAction action, ReceiptRequestDto request);
    }
}