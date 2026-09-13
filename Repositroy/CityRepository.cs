using Dapper;
using Microsoft.Data.SqlClient;
using server.Dto;
using server.Enums;
using server.Interfaces.Repository;
using System.Data;

namespace server.Repository
{
    public class CityRepository : ICityRepository
    {
        private readonly IConfiguration _configuration;

        public CityRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<CityDto>> ExecuteProcedureAsync(
            string username,
            CityAction action,
            CityRequestDto request)
        {
            var connectionString =
                _configuration.GetConnectionString("DefaultConnection");

            using var connection = new SqlConnection(connectionString);

            var parameters = new DynamicParameters();

            parameters.Add("@p_user", username);
            parameters.Add("@p_action", action.ToString());
            parameters.Add("@p_formid", request.formId);
            parameters.Add("@p_jsondata", request.data.ToString());
            parameters.Add("@p_record_id", request.recordId);

            var result = await connection.QueryAsync<CityDto>(
                "SP_CITY_SETUP",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }
    }
}