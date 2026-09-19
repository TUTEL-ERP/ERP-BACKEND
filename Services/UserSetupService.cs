using server.Dto;
using server.Enums;
using server.Interfaces.Repository;
using server.Interfaces.Services;
using System.Data;

namespace server.Services
{
    public class UserSetupService : IUserSetupService
    {
        private readonly IUserSetupRepository _repo;
        private readonly ILogger<UserSetupService> _logger;

        public UserSetupService(IUserSetupRepository repo, ILogger<UserSetupService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public Task<DataTable> GetGridDataAsync(string u, UserRequestDto r) => _repo.ExecuteProcedureAsync(u, UserAction.GRIDDATA, r);
        public Task<DataTable> InsertRecordAsync(string u, UserRequestDto r) => _repo.ExecuteProcedureAsync(u, UserAction.INSERT, r);
        public Task<DataTable> UpdateRecordAsync(string u, UserRequestDto r) => _repo.ExecuteProcedureAsync(u, UserAction.UPDATE, r);
        public Task<DataTable> DeleteRecordAsync(string u, UserRequestDto r) => _repo.ExecuteProcedureAsync(u, UserAction.DELETE, r);
        public Task<DataTable> SelectRecordAsync(string u, UserRequestDto r) => _repo.ExecuteProcedureAsync(u, UserAction.SINGELRECORD, r);
    }
}