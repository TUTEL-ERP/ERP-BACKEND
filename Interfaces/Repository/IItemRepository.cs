using server.Dto;
using server.Enums;
using System.Data;

namespace server.Interfaces.Repository
{
    public interface IItemRepository
    {
        Task<DataTable> ExecuteProcedureAsync(string usrname, ItemAction action, ItemRequestDto request);
    }
}