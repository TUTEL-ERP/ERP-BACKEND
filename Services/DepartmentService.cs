using server.Dto;
using server.Enums;
using server.Interfaces.Repository;
using server.Interfaces.Services;
using System.Data;

namespace server.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _repo;
        private readonly ILogger<DepartmentService> _logger;

        public DepartmentService(IDepartmentRepository repo, ILogger<DepartmentService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public Task<DataTable> GetGridDataAsync(string u, DepartmentRequestDto r) => _repo.ExecuteProcedureAsync(u, DepartmentAction.GRIDDATA, r);
        public Task<DataTable> InsertRecordAsync(string u, DepartmentRequestDto r) => _repo.ExecuteProcedureAsync(u, DepartmentAction.INSERT, r);
        public Task<DataTable> UpdateRecordAsync(string u, DepartmentRequestDto r) => _repo.ExecuteProcedureAsync(u, DepartmentAction.UPDATE, r);
        public Task<DataTable> DeleteRecordAsync(string u, DepartmentRequestDto r) => _repo.ExecuteProcedureAsync(u, DepartmentAction.DELETE, r);
        public Task<DataTable> SelectRecordAsync(string u, DepartmentRequestDto r) => _repo.ExecuteProcedureAsync(u, DepartmentAction.SINGELRECORD, r);
    }
}