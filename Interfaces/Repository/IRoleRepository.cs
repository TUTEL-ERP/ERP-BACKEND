using server.Dto;
using server.Enums;
using System.Data;

namespace server.Interfaces.Repository
{
    public interface IRoleRepository
    {
        Task<DataSet> ExecuteProcedureAsync(string usrname, RoleAction action, RoleRequestDto request);
    }
}