using server.Dto;
using server.Enums;
using server.Interfaces.Repository;
using server.Interfaces.Services;
using System.Data;

namespace server.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _repo;
        private readonly ILogger<ItemService> _logger;

        public ItemService(IItemRepository repo, ILogger<ItemService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public Task<DataTable> GetGridDataAsync(string u, ItemRequestDto r) => _repo.ExecuteProcedureAsync(u, ItemAction.GRIDDATA, r);
        public Task<DataTable> InsertRecordAsync(string u, ItemRequestDto r) => _repo.ExecuteProcedureAsync(u, ItemAction.INSERT, r);
        public Task<DataTable> UpdateRecordAsync(string u, ItemRequestDto r) => _repo.ExecuteProcedureAsync(u, ItemAction.UPDATE, r);
        public Task<DataTable> DeleteRecordAsync(string u, ItemRequestDto r) => _repo.ExecuteProcedureAsync(u, ItemAction.DELETE, r);
        public Task<DataTable> SelectRecordAsync(string u, ItemRequestDto r) => _repo.ExecuteProcedureAsync(u, ItemAction.SINGELRECORD, r);
    }
}