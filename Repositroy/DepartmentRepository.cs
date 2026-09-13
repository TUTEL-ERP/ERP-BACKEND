using ERP_API.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using server.Dto;
using server.Enums;
using server.Interfaces.Repository;
using System.Data;

namespace server.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;

        public DepartmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataTable> ExecuteProcedureAsync(string usrname, DepartmentAction action, DepartmentRequestDto request)
        {
            using var connection = new SqlConnection(_context.Database.GetConnectionString());
            using var cmd = new SqlCommand("SP_DEPARTMENT_SETUP", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_user", usrname);
            cmd.Parameters.AddWithValue("@p_action", action.ToString());
            cmd.Parameters.AddWithValue("@p_formid", request.formId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@p_jsondata", request.data.ToString() ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@p_record_id", request.recordId ?? (object)DBNull.Value);

            await connection.OpenAsync();
            var dt = new DataTable();
            using var reader = await cmd.ExecuteReaderAsync();
            dt.Load(reader);
            return dt;
        }
    }
}