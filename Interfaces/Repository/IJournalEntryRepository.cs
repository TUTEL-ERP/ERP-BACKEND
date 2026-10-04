using server.Dto;
using server.Enums;
using System.Data;

namespace server.Interfaces.Repository
{
    public interface IJournalEntryRepository
    {
        Task<DataSet> ExecuteProcedureAsync(string usrname, JournalEntryAction action, JournalEntryRequestDto request);
    }
}