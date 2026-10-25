using server.Dto;
using server.Enums;
using System.Data;

namespace server.Interfaces.Repository
{
    public interface IDepartmentalIssueRepository
    {
        Task<DataSet> ExecuteProcedureAsync(
            string usrname,
            DepartmentalIssueAction action,
            DepartmentalIssueDto request);
        Task<DataSet> GetItemBatchesAsync(string usrname, DepartmentalIssueDto request);
    }
}
