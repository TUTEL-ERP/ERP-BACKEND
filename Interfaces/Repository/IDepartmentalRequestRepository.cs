using server.Dto;
using server.Enums;
using System.Data;

namespace server.Interfaces.Repository
{
    public interface IDepartmentalRequestRepository
    {
        Task<DataSet> ExecuteProcedureAsync(string usrname, DepartmentalRequestAction action, DepartmentalRequestDto request);
    }
}