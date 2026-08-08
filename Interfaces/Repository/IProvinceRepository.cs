using server.Dto;
using server.Enums;
using System.Data;

namespace server.Interfaces.Repository
{
    public interface IProvinceRepository
    {
        Task<DataTable> ExecuteProcedureAsync(string usrname, ProvinceAction action, ProvinceRequestDto request);
    }
}