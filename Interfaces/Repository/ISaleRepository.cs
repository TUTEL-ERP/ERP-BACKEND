using server.Dto;
using server.Enums;
using System.Data;

namespace server.Interfaces.Repository
{
    public interface ISaleRepository
    {
        Task<DataSet> ExecuteProcedureAsync(string usrname, SaleAction action, SaleRequestDto request);
    }
}