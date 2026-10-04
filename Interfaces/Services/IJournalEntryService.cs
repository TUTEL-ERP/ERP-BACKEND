using server.Dto;
using System.Data;

namespace server.Interfaces.Services
{
    public interface IJournalEntryService
    {
        Task<DataTable> GetGridDataAsync(string u, JournalEntryRequestDto r);
        Task<DataSet> GetMasterDetailAsync(string u, JournalEntryRequestDto r);
        Task<DataTable> InsertRecordAsync(string u, JournalEntryRequestDto r);
        Task<DataTable> UpdateRecordAsync(string u, JournalEntryRequestDto r);
        Task<DataTable> DeleteRecordAsync(string u, JournalEntryRequestDto r);
        Task<DataTable> GetAccountsAsync(string u, JournalEntryRequestDto r);
        Task<string> GetNextJournalNumberAsync(string u, JournalEntryRequestDto r);
    }
}