namespace ReportingService.Models
{
    public class UserActivityReport
    {
        public int UserId { get; set; }
        public string Action { get; set; }
        public DateTime ActionTime { get; set; }
    }
}
