using server.Dto;
using server.Enums;
using server.Interfaces.Repository;
using server.Interfaces.Services;
using System.Data;

namespace server.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repo;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(ICustomerRepository repo, ILogger<CustomerService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public Task<DataTable> GetGridDataAsync(string u, CustomerRequestDto r) => _repo.ExecuteProcedureAsync(u, CustomerAction.GRIDDATA, r);
        public Task<DataTable> InsertRecordAsync(string u, CustomerRequestDto r) => _repo.ExecuteProcedureAsync(u, CustomerAction.INSERT, r);
        public Task<DataTable> UpdateRecordAsync(string u, CustomerRequestDto r) => _repo.ExecuteProcedureAsync(u, CustomerAction.UPDATE, r);
        public Task<DataTable> DeleteRecordAsync(string u, CustomerRequestDto r) => _repo.ExecuteProcedureAsync(u, CustomerAction.DELETE, r);
        public Task<DataTable> SelectRecordAsync(string u, CustomerRequestDto r) => _repo.ExecuteProcedureAsync(u, CustomerAction.SINGELRECORD, r);
    }
}