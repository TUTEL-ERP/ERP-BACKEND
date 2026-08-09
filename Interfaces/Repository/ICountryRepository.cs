using server.Dto;
using server.Enity;
using server.Enums;
using System.Data;

namespace server.Interfaces.Repository
{
    public interface ICountryRepository : IGenericRepository<Country>
    {
   
        Task<DataTable> ExecuteProcedureAsync(string usrname, CountryAction action, CountryRequestDto request);
    }
}