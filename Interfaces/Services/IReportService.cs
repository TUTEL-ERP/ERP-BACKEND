using System.Data;

namespace server.Interfaces.Services
{
    public interface IReportService
    {
        Task<DataTable> GetTrialBalanceAsync(
            string userId,
            DateOnly fromDate,
            DateOnly toDate,
            CancellationToken cancellationToken);

        Task<DataTable> GetBalanceSheetAsync(
            string userId,
            DateOnly asOfDate,
            CancellationToken cancellationToken);
    }
}
