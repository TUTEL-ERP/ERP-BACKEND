using server.Dto;
using server.Enums;
using System.Data;

namespace server.Interfaces.Repository
{
    public interface ICompanyRepository
    {
        Task<DataTable> ExecuteProcedureAsync(string usrname, CompanyAction action, CompanyRequestDto request);
    }
}