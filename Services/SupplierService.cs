using server.Dto;
using server.Enums;
using server.Interfaces.Repository;
using server.Interfaces.Services;
using System.Data;

namespace server.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _repo;
        private readonly ILogger<SupplierService> _logger;

        public SupplierService(ISupplierRepository repo, ILogger<SupplierService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public Task<DataTable> GetGridDataAsync(string u, SupplierRequestDto r) => _repo.ExecuteProcedureAsync(u, SupplierAction.GRIDDATA, r);
        public Task<DataTable> InsertRecordAsync(string u, SupplierRequestDto r) => _repo.ExecuteProcedureAsync(u, SupplierAction.INSERT, r);
        public Task<DataTable> UpdateRecordAsync(string u, SupplierRequestDto r) => _repo.ExecuteProcedureAsync(u, SupplierAction.UPDATE, r);
        public Task<DataTable> DeleteRecordAsync(string u, SupplierRequestDto r) => _repo.ExecuteProcedureAsync(u, SupplierAction.DELETE, r);
        public Task<DataTable> SelectRecordAsync(string u, SupplierRequestDto r) => _repo.ExecuteProcedureAsync(u, SupplierAction.SINGELRECORD, r);
    }
}