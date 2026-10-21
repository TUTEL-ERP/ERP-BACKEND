using server.Dto;
using server.Enums;
using System.Data;

namespace server.Interfaces.Repository
{
    public interface IGRNRepository
    {
        Task<DataSet> ExecuteProcedureAsync(string usrname, GRNAction action, GRNRequestDto request);
    }
}