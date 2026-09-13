using server.Dto;
using server.Enums;
using System.Data;

namespace server.Interfaces.Repository
{
    public interface ISupplierRepository
    {
        Task<DataTable> ExecuteProcedureAsync(string usrname, SupplierAction action, SupplierRequestDto request);
    }
}