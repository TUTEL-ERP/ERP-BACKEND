using server.Dto;
using server.Enums;
using System.Data;

namespace server.Interfaces.Repository
{
    public interface IAreaRepository
    {
        Task<DataTable> ExecuteProcedureAsync(string usrname, AreaAction action, AreaRequestDto request);
    }
}