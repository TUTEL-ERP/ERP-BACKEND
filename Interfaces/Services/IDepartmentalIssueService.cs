using server.Dto;
using System.Data;

namespace server.Interfaces.Services
{
    public interface IDepartmentalIssueService
    {
        Task<DataTable> GetGridDataAsync(string usrname, DepartmentalIssueDto request);
        Task<DataSet> GetMasterDetailAsync(string usrname, DepartmentalIssueDto request);
        Task<DataTable> InsertRecordAsync(string usrname, DepartmentalIssueDto request);
        Task<DataTable> UpdateRecordAsync(string usrname, DepartmentalIssueDto request);
        Task<DataTable> DeleteRecordAsync(string usrname, DepartmentalIssueDto request);
        Task<DataTable> GetDepartmentsAsync(string usrname, DepartmentalIssueDto request);
        Task<DataTable> GetItemsAsync(string usrname, DepartmentalIssueDto request);
        Task<string> GetNextNoAsync(string usrname, DepartmentalIssueDto request);
        Task<DataTable> GetApprovedDRsAsync(string usrname, DepartmentalIssueDto request);
        Task<DataTable> GetDRItemsAsync(string usrname, DepartmentalIssueDto request);
        Task<DataTable> ApproveAsync(string usrname, DepartmentalIssueDto request);
        Task<DataTable> RejectAsync(string usrname, DepartmentalIssueDto request);
    }
}
