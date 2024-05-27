using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReportingService.Services;

namespace ReportingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("flight")]
        public async Task<IActionResult> GetFlightReport(DateTime startDate, DateTime endDate)
        {
            var report = await _reportService.GetFlightReportAsync(startDate, endDate);
            return Ok(report);
        }

        [HttpGet("document")]
        public async Task<IActionResult> GetDocumentReport(DateTime startDate, DateTime endDate)
        {
            var report = await _reportService.GetDocumentReportAsync(startDate, endDate);
            return Ok(report);
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetUserReport(DateTime startDate, DateTime endDate)
        {
            var report = await _reportService.GetUserReportAsync(startDate, endDate);
            return Ok(report);
        }

        [HttpGet("user-activity")]
        public async Task<IActionResult> GetUserActivityReport(DateTime startDate, DateTime endDate)
        {
            var report = await _reportService.GetUserActivityReportAsync(startDate, endDate);
            return Ok(report);
        }
    }
}
