using server.Dto;
using server.Enums;
using System.Data;

namespace server.Interfaces.Repository
{
    public interface ISaleReturnRepository
    {
        Task<DataSet> ExecuteProcedureAsync(string usrname, SaleReturnAction action, SaleReturnRequestDto request);
    }
}