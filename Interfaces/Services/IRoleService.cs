using server.Dto;
using System.Data;

namespace server.Interfaces.Services
{
    public interface IRoleService
    {
        Task<DataTable> GetGridDataAsync(string u, RoleRequestDto r);
        Task<DataSet> GetMasterDetailAsync(string u, RoleRequestDto r);
        Task<DataTable> InsertRecordAsync(string u, RoleRequestDto r);
        Task<DataTable> UpdateRecordAsync(string u, RoleRequestDto r);
        Task<DataTable> DeleteRecordAsync(string u, RoleRequestDto r);
        Task<DataTable> GetPermissionsAsync(string u, RoleRequestDto r);
        Task<DataTable> GetFormsAsync(string u, RoleRequestDto r);
        Task<string> GetNextRoleCodeAsync(string u, RoleRequestDto r);
    }
}