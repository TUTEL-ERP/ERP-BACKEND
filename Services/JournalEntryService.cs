using server.Dto;
using server.Enums;
using server.Interfaces.Repository;
using server.Interfaces.Services;
using System.Data;

namespace server.Services
{
    public class JournalEntryService : IJournalEntryService
    {
        private readonly IJournalEntryRepository _repo;
        private readonly ILogger<JournalEntryService> _logger;

        public JournalEntryService(IJournalEntryRepository repo, ILogger<JournalEntryService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<DataTable> GetGridDataAsync(string u, JournalEntryRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, JournalEntryAction.GRIDDATA, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataSet> GetMasterDetailAsync(string u, JournalEntryRequestDto r)
            => await _repo.ExecuteProcedureAsync(u, JournalEntryAction.SINGELRECORD, r);

        public async Task<DataTable> InsertRecordAsync(string u, JournalEntryRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, JournalEntryAction.INSERT, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> UpdateRecordAsync(string u, JournalEntryRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, JournalEntryAction.UPDATE, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> DeleteRecordAsync(string u, JournalEntryRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, JournalEntryAction.DELETE, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<DataTable> GetAccountsAsync(string u, JournalEntryRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, JournalEntryAction.GET_ACCOUNTS, r);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public async Task<string> GetNextJournalNumberAsync(string u, JournalEntryRequestDto r)
        {
            var ds = await _repo.ExecuteProcedureAsync(u, JournalEntryAction.GET_NEXT_JOURNAL_NUMBER, r);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][0]?.ToString() ?? "";
            return "";
        }
    }
}