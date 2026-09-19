using server.Dto;
using server.Enums;
using System.Data;

namespace server.Interfaces.Repository
{
    public interface ICustomerRepository
    {
        Task<DataTable> ExecuteProcedureAsync(string usrname, CustomerAction action, CustomerRequestDto request);
    }
}