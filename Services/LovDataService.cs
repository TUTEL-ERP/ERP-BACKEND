using Microsoft.Data.SqlClient;
using System.Data;

namespace Lov.Application.Service
{
    public class LovDataService : ILovDataService
    {
        private readonly string _connectionString;

        public LovDataService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<DataTable> GetLovData(string user, string lovCode, string? action)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@p_lovcode", lovCode),
                new SqlParameter("@p_user", user)
            };

            DataTable masterTable = await ExecuteSpAsync("SP_LOV_MASTER", parameters);

            if (masterTable.Rows.Count > 0 && masterTable.Rows[0][0].ToString() == "0")
            {
                string procedureName = masterTable.Rows[0]["procedureName"].ToString();
                string finalAction = action ?? masterTable.Rows[0]["actionName"].ToString();

                // ✅ STEP 2: Call the actual SP
                parameters = new List<SqlParameter>
                {
                    new SqlParameter("@p_user", user),
                    new SqlParameter("@p_action", finalAction)
                };

                DataTable dataTable = await ExecuteSpAsync(procedureName, parameters);
                return dataTable;
            }
            else
            {
                return masterTable;
            }
        }

        private async Task<DataTable> ExecuteSpAsync(string spName, List<SqlParameter> parameters)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(spName, connection);
            cmd.CommandType = CommandType.StoredProcedure;

            foreach (var p in parameters)
                cmd.Parameters.Add(p);

            await connection.OpenAsync();
            var dt = new DataTable();
            using var reader = await cmd.ExecuteReaderAsync();
            dt.Load(reader);
            return dt;
        }
    }
}