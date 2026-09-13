using server.Dto;
using server.Enums;
using System.Data;

namespace server.Interfaces.Repository
{
    public interface IBranchRepository
    {
        Task<DataTable> ExecuteProcedureAsync(string usrname, BranchAction action, BranchRequestDto request);
    }
}