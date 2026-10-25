using ERP_API.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using server.Dto;
using server.Enums;
using server.Interfaces.Repository;
using System.Data;
using System.Text.Json;

namespace server.Repository
{
    public class DepartmentalIssueRepository : IDepartmentalIssueRepository
    {
        private readonly ApplicationDbContext _context;

        public DepartmentalIssueRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataSet> ExecuteProcedureAsync(
            string usrname,
            DepartmentalIssueAction action,
            DepartmentalIssueDto request)
        {
            using var connection = new SqlConnection(_context.Database.GetConnectionString());
            using var command = new SqlCommand("SP_DEPARTMENTAL_ISSUE_SETUP", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@p_user", SqlDbType.NVarChar, 50).Value = usrname;
            command.Parameters.Add("@p_action", SqlDbType.NVarChar, 50).Value =
                action == DepartmentalIssueAction.GET_NEXT_NO ? "GET_NEXT_ISSUE_NO" : action.ToString();
            command.Parameters.Add("@p_formid", SqlDbType.NVarChar, 50).Value =
                (object?)request.formId ?? DBNull.Value;
            command.Parameters.Add("@p_jsondata", SqlDbType.NVarChar, -1).Value =
                request.data.ValueKind == JsonValueKind.Undefined ? "{}" : request.data.GetRawText();

            var recordIdParameter = command.Parameters.Add("@p_record_id", SqlDbType.BigInt);
            if (string.IsNullOrWhiteSpace(request.recordId))
            {
                recordIdParameter.Value = DBNull.Value;
            }
            else if (long.TryParse(request.recordId, out var recordId))
            {
                recordIdParameter.Value = recordId;
            }
            else
            {
                throw new ArgumentException("recordId must be a valid 64-bit integer.", nameof(request));
            }

            await connection.OpenAsync();
            var dataSet = new DataSet();
            using var reader = await command.ExecuteReaderAsync();
            dataSet.Load(reader, LoadOption.PreserveChanges, "Master", "Lines");
            return dataSet;
        }
    }
}
