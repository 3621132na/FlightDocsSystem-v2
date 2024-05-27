namespace ReportingService.Models
{
    public class DocumentReport
    {
        public string DocumentType { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int DocumentCount { get; set; }
    }
}
