using server.Dto;
using server.Enums;
using System.Data;

namespace server.Interfaces.Repository
{
    public interface IPermissionRepository
    {
        Task<DataSet> ExecuteProcedureAsync(string usrname, PermissionAction action, PermissionRequestDto request);
    }
}