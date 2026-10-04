using server.Dto;
using server.Enums;
using server.Interfaces.Repository;
using server.Interfaces.Services;
using System.Data;

namespace server.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repo;
        private readonly ILogger<RoleService> _logger;

        public RoleService(IRoleRepository repo, ILogger<RoleService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<DataTable> GetGridDataAsync(string u, RoleRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, RoleAction.GRIDDATA, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataSet> GetMasterDetailAsync(string u, RoleRequestDto r)
            => await _repo.ExecuteProcedureAsync(u, RoleAction.SINGELRECORD, r);

        public async Task<DataTable> InsertRecordAsync(string u, RoleRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, RoleAction.INSERT, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> UpdateRecordAsync(string u, RoleRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, RoleAction.UPDATE, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> DeleteRecordAsync(string u, RoleRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, RoleAction.DELETE, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> GetPermissionsAsync(string u, RoleRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, RoleAction.GET_PERMISSIONS, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> GetFormsAsync(string u, RoleRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, RoleAction.GET_FORMS, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<string> GetNextRoleCodeAsync(string u, RoleRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, RoleAction.GET_NEXT_ROLE_CODE, r);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][0]?.ToString() ?? "";
            return "";
        }
    }
}