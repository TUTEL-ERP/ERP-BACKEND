using server.Dto;
using server.Enums;
using server.Interfaces.Repository;
using server.Interfaces.Services;
using System.Data;
using System.Text.Json;

namespace server.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _repo;
        private readonly ILogger<PermissionService> _logger;

        public PermissionService(IPermissionRepository repo, ILogger<PermissionService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<PermissionResponseDto?> LoadForUserAsync(string userid, string formId)
        {
            var request = new PermissionRequestDto
            {
                formId = formId,
                data = JsonDocument.Parse("{}").RootElement
            };

            var ds = await _repo.ExecuteProcedureAsync(userid, PermissionAction.LOAD_FOR_USER, request);

            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return null;

            return MapRow(ds.Tables[0].Rows[0], formId);
        }

        public async Task<List<PermissionResponseDto>> GetAllForUserAsync(string userid)
        {
            var request = new PermissionRequestDto
            {
                data = JsonDocument.Parse("{}").RootElement
            };

            var ds = await _repo.ExecuteProcedureAsync(userid, PermissionAction.GET_ALL_FOR_USER, request);
            var list = new List<PermissionResponseDto>();
            if (ds.Tables.Count == 0) return list;

            foreach (DataRow row in ds.Tables[0].Rows)
                list.Add(MapRow(row, row["formId"]?.ToString() ?? ""));

            return list;
        }

        public async Task<bool> CheckPermissionAsync(string userid, string formId, string permissionCode)
        {
            var perms = await LoadForUserAsync(userid, formId);
            if (perms == null) return false;

            return permissionCode.ToUpper() switch
            {
                "VIEW" => perms.View,
                "ADD" => perms.Add,
                "EDIT" => perms.Edit,
                "DELETE" => perms.Delete,
                "APPROVE" => perms.Approve,
                "REJECT" => perms.Reject,
                "PRINT" => perms.Print,
                _ => false
            };
        }

        private static PermissionResponseDto MapRow(DataRow row, string fallbackFormId)
        {
            return new PermissionResponseDto
            {
                FormId = row["formId"]?.ToString() ?? fallbackFormId,
                RoleCode = row.Table.Columns.Contains("roleCode") ? row["roleCode"]?.ToString() : null,
                IsTransaction = row.Table.Columns.Contains("isTransaction") && row["isTransaction"] != DBNull.Value
                                ? Convert.ToBoolean(row["isTransaction"]) : false,
                View = Convert.ToBoolean(row["view"]),
                Add = Convert.ToBoolean(row["add"]),
                Edit = Convert.ToBoolean(row["edit"]),
                Delete = Convert.ToBoolean(row["delete"]),
                Approve = Convert.ToBoolean(row["approve"]),
                Reject = Convert.ToBoolean(row["reject"]),
                Print = Convert.ToBoolean(row["print"])
            };
        }
    }
}