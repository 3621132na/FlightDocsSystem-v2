using ReportingService.Models;

namespace ReportingService.Services
{
    public interface IReportService
    {
        Task<IEnumerable<FlightReport>> GetFlightReportAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<DocumentReport>> GetDocumentReportAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<UserReport>> GetUserReportAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<UserActivityReport>> GetUserActivityReportAsync(DateTime startDate, DateTime endDate);
    }
}
