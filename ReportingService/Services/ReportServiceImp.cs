using Microsoft.EntityFrameworkCore;
using ReportingService.Data;
using ReportingService.Models;
using System.Xml.Serialization;

namespace ReportingService.Services
{
    public class ReportServiceImp : IReportService
    {
        private readonly ReportContext _context;

        public ReportServiceImp(ReportContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FlightReport>> GetFlightReportAsync(DateTime startDate, DateTime endDate)
        {
            var reports = await _context.Flights
                .Include(f => f.Users)
                .Where(f => f.DepartureDate >= startDate && f.DepartureDate <= endDate)
                .GroupBy(f => new { f.DepartureDate.Date, f.AircraftType, f.Status })
                .Select(g => new FlightReport
                {
                    DepartureDate = g.Key.Date,
                    AircraftType = g.Key.AircraftType,
                    Status = g.Key.Status,
                    FlightCount = g.Count(),
                    TotalUsers = g.SelectMany(f => f.Users).Count()
                })
                .ToListAsync();

            return reports;
        }

        public async Task<IEnumerable<DocumentReport>> GetDocumentReportAsync(DateTime startDate, DateTime endDate)
        {
            var reports = await _context.Documents
                .Where(d => d.CreatedAt >= startDate && d.CreatedAt <= endDate)
                .GroupBy(d => new { d.DocumentType, d.CreatedBy, d.UpdatedAt })
                .Select(g => new DocumentReport
                {
                    DocumentType = g.Key.DocumentType,
                    CreatedBy = g.Key.CreatedBy,
                    UpdatedDate = g.Key.UpdatedAt,
                    DocumentCount = g.Count()
                })
                .ToListAsync();

            return reports;
        }

        public async Task<IEnumerable<UserReport>> GetUserReportAsync(DateTime startDate, DateTime endDate)
        {
            var reports = await _context.Users
                .Where(u => u.Role != null)
                .GroupBy(u => u.Role)
                .Select(g => new UserReport
                {
                    Role = g.Key,
                    UserCount = g.Count()
                })
                .ToListAsync();
            return reports;
        }

        public async Task<IEnumerable<UserActivityReport>> GetUserActivityReportAsync(DateTime startDate, DateTime endDate)
        {
            var reports = await _context.UserActivities
                .Where(a => a.ActionTime >= startDate && a.ActionTime <= endDate)
                .Select(a => new UserActivityReport
                {
                    UserId = a.UserId,
                    Action = a.Action,
                    ActionTime = a.ActionTime
                })
                .ToListAsync();

            return reports;
        }
    }
}
