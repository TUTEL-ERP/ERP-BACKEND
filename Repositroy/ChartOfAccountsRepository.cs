using ERP_API.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using server.Dto;
using server.Enums;
using server.Interfaces.Repository;
using System.Data;

namespace server.Repository
{
    public class ChartOfAccountsRepository : IChartOfAccountsRepository
    {
        private readonly ApplicationDbContext _context;

        public ChartOfAccountsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataSet> ExecuteProcedureAsync(string usrname, ChartOfAccountsAction action, ChartOfAccountsRequestDto request)
        {
            using var connection = new SqlConnection(_context.Database.GetConnectionString());
            using var cmd = new SqlCommand("SP_CHART_OF_ACCOUNTS_SETUP", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_user", usrname);
            cmd.Parameters.AddWithValue("@p_action", action.ToString());
            cmd.Parameters.AddWithValue("@p_formid", request.formId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@p_jsondata", request.data.ToString() ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@p_record_id", request.recordId ?? (object)DBNull.Value);

            await connection.OpenAsync();
            var ds = new DataSet();
            using var reader = await cmd.ExecuteReaderAsync();
            ds.Load(reader, LoadOption.PreserveChanges, "Master", "Lines");
            return ds;
        }
    }
}