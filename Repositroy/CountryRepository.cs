using ERP_API.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using server.Dto;
using server.Enity;
using server.Enums;
using server.Interfaces.Repository;
using System.Data;

namespace server.Repository
{
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        private readonly ApplicationDbContext _context;

        public CountryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        // ✅ Removed: GetAllCountriesAsync, GetCountryByIdAsync, GetActiveCountriesAsync, 
        // IsCountryExistsAsync, CreateCountryAsync, UpdateCountryAsync, DeleteCountryAsync

        // ✅ ONLY THIS REMAINS:
        public async Task<DataTable> ExecuteProcedureAsync(string usrname, CountryAction action, CountryRequestDto request)
        {
            using var connection = new SqlConnection(_context.Database.GetConnectionString());
            using var cmd = new SqlCommand("SP_COUNTRY_SETUP", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_user", usrname);
            cmd.Parameters.AddWithValue("@p_action", action.ToString());
            cmd.Parameters.AddWithValue("@p_formid", request.formId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@p_jsondata", request.data.ToString() ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@p_record_id", request.recordId ?? (object)DBNull.Value);

            await connection.OpenAsync();

            var dataTable = new DataTable();
            using var reader = await cmd.ExecuteReaderAsync();
            dataTable.Load(reader);
            return dataTable;
        }
    }
}