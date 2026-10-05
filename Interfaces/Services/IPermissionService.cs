using server.Dto;

namespace server.Interfaces.Services
{
    public interface IPermissionService
    {
        Task<PermissionResponseDto?> LoadForUserAsync(string userid, string formId);
        Task<List<PermissionResponseDto>> GetAllForUserAsync(string userid);
        Task<bool> CheckPermissionAsync(string userid, string formId, string permissionCode);
    }
}