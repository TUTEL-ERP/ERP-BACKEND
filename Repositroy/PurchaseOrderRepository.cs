using ERP_API.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using server.Dto;
using server.Enums;
using server.Interfaces.Repository;
using System.Data;

namespace server.Repository
{
    public class PurchaseOrderRepository : IPurchaseOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public PurchaseOrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataSet> ExecuteProcedureAsync(string usrname, PurchaseOrderAction action, PurchaseOrderRequestDto request)
        {
            using var connection = new SqlConnection(_context.Database.GetConnectionString());
            using var cmd = new SqlCommand("SP_PURCHASE_ORDER_SETUP", connection);
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