using ERP_API.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using server.Dto;
using server.Interfaces.Services;
using System.Data;
using System.Text.Json;

namespace server.Services
{
    public sealed class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<DataTable> GetTrialBalanceAsync(
            string userId,
            DateOnly fromDate,
            DateOnly toDate,
            CancellationToken cancellationToken)
        {
            var report = new ReportDto
            {
                FromDate = fromDate,
                ToDate = toDate
            };

            return ExecuteReportAsync(userId, "TRIAL_BALANCE", report, cancellationToken);
        }

        public Task<DataTable> GetBalanceSheetAsync(
            string userId,
            DateOnly asOfDate,
            CancellationToken cancellationToken)
        {
            var report = new ReportDto
            {
                AsOfDate = asOfDate
            };

            return ExecuteReportAsync(userId, "BALANCE_SHEET", report, cancellationToken);
        }

        private async Task<DataTable> ExecuteReportAsync(
            string userId,
            string action,
            ReportDto report,
            CancellationToken cancellationToken)
        {
            await using var connection = new SqlConnection(_context.Database.GetConnectionString());
            await using var command = new SqlCommand("SP_REPORT_GL_SETUP", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@p_user", userId);
            command.Parameters.AddWithValue("@p_action", action);
            command.Parameters.AddWithValue("@p_formid", ReportDto.FormId);
            command.Parameters.AddWithValue("@p_jsondata", JsonSerializer.Serialize(report));
            command.Parameters.AddWithValue("@p_record_id", DBNull.Value);

            await connection.OpenAsync(cancellationToken);

            var result = new DataTable();
            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            result.Load(reader);

            return result;
        }
    }
}
