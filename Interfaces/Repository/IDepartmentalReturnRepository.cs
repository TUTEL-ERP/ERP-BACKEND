using server.Dto;
using server.Enums;
using System.Data;

namespace server.Interfaces.Repository
{
    public interface IDepartmentalReturnRepository
    {
        Task<DataSet> ExecuteProcedureAsync(string user, DepartmentalReturnAction action, DepartmentalReturnDto request);
    }
}
