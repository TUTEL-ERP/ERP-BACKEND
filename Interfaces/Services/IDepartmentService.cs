using server.Dto;
using System.Data;

namespace server.Interfaces.Services
{
    public interface IDepartmentService
    {
        Task<DataTable> GetGridDataAsync(string usrname, DepartmentRequestDto request);
        Task<DataTable> InsertRecordAsync(string usrname, DepartmentRequestDto request);
        Task<DataTable> UpdateRecordAsync(string usrname, DepartmentRequestDto request);
        Task<DataTable> DeleteRecordAsync(string usrname, DepartmentRequestDto request);
        Task<DataTable> SelectRecordAsync(string usrname, DepartmentRequestDto request);
    }
}