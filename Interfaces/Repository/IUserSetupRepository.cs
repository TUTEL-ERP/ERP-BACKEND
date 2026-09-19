using server.Dto;
using server.Enums;
using System.Data;

namespace server.Interfaces.Repository
{
    public interface IUserSetupRepository
    {
        Task<DataTable> ExecuteProcedureAsync(string usrname, UserAction action, UserRequestDto request);
    }
}