namespace AuditService.Models
{
    public class AuditLog
    {
        public int AuditLogId { get; set; }
        public int UserId { get; set; }
        public string Action { get; set; }
        public DateTime ActionDate { get; set; }
        public string Details { get; set; }
    }
}
