using server.Dto;
using server.Enums;
using server.Interfaces.Repository;
using server.Interfaces.Services;
using server.Repository;
using System.Data;

namespace server.Services
{
    public class BranchService : IBranchService
    {
        private readonly IBranchRepository _repo;
        private readonly ILogger<BranchService> _logger;

        public BranchService(IBranchRepository repo, ILogger<BranchService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public Task<DataTable> GetGridDataAsync(string u, BranchRequestDto r) => _repo.ExecuteProcedureAsync(u, BranchAction.GRIDDATA, r);
        public Task<DataTable> GetLovDataAsync(string u, BranchRequestDto r) => _repo.ExecuteProcedureAsync(u, BranchAction.LOV, r);
        public Task<DataTable> InsertRecordAsync(string u, BranchRequestDto r) => _repo.ExecuteProcedureAsync(u, BranchAction.INSERT, r);
        public Task<DataTable> UpdateRecordAsync(string u, BranchRequestDto r) => _repo.ExecuteProcedureAsync(u, BranchAction.UPDATE, r);
        public Task<DataTable> DeleteRecordAsync(string u, BranchRequestDto r) => _repo.ExecuteProcedureAsync(u, BranchAction.DELETE, r);
        public Task<DataTable> SelectRecordAsync(string u, BranchRequestDto r) => _repo.ExecuteProcedureAsync(u, BranchAction.SINGELRECORD, r);
    }
}