using server.Dto;
using server.Enums;
using System.Data;

namespace server.Interfaces.Repository
{
    public interface ICityRepository
    {
        Task<List<CityDto>> ExecuteProcedureAsync(
            string username,
            CityAction action,
            CityRequestDto request);
    }
}