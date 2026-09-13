using System.Data;

namespace Lov.Application.Service
{
    public interface ILovDataService
    {
        Task<DataTable> GetLovData(string user, string lovCode, string? action);
    }
}