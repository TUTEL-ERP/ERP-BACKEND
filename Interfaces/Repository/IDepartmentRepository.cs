using server.Dto;
using server.Enums;
using System.Data;

namespace server.Interfaces.Repository
{
    public interface IDepartmentRepository
    {
        Task<DataTable> ExecuteProcedureAsync(string usrname, DepartmentAction action, DepartmentRequestDto request);
    }
}